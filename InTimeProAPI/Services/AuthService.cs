using InTimeProAPI.Data;
using InTimeProAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace InTimeProAPI.Services;

public class AuthService : IAuthService
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutWindow = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan ResetTokenLifetime = TimeSpan.FromMinutes(15);
    private static readonly Regex PasswordComplexityRegex =
        new("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\w\\s]).{8,}$", RegexOptions.Compiled);
    private static readonly ConcurrentDictionary<string, PasswordResetTicket> ResetTickets = new();

    private readonly AppDbContext _db;
    private readonly ILogger<AuthService> _logger;
    private readonly IAlertingService _alertingService;
    private readonly IAuditLogService _auditLogService;

    public AuthService(
        AppDbContext db,
        ILogger<AuthService> logger,
        IAlertingService alertingService,
        IAuditLogService auditLogService)
    {
        _db = db;
        _logger = logger;
        _alertingService = alertingService;
        _auditLogService = auditLogService;
    }

    public async Task<(Employee? employee, string? error)> ValidateEmailPasswordAsync(
        string email, string password)
    {
        var employee = await _db.Employees
            .FirstOrDefaultAsync(e => e.Email == email.ToLower() && e.IsActive);

        if (employee == null)
            return (null, "Invalid email or password");

        if (employee.LockoutEndUtc.HasValue && employee.LockoutEndUtc.Value > DateTime.UtcNow)
            return (null, "Account is temporarily locked. Please try again later.");

        if (employee.PasswordHash == null)
            return (null, "This account uses social login. Please use Google or Microsoft to sign in.");

        bool passwordValid = BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash);
        if (!passwordValid)
        {
            employee.FailedLoginCount += 1;
            if (employee.FailedLoginCount >= MaxFailedAttempts)
            {
                employee.LockoutEndUtc = DateTime.UtcNow.Add(LockoutWindow);
                employee.FailedLoginCount = 0;
            }

            employee.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await _alertingService.AlertFailedLoginAsync(email, "Invalid password");
            return (null, "Invalid email or password");
        }

        employee.FailedLoginCount = 0;
        employee.LockoutEndUtc = null;
        employee.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        await _auditLogService.WriteAsync("Auth.Login", "Employee", employee.Id.ToString(), new
        {
            employee.Email,
            Provider = "email"
        });

        return (employee, null);
    }

    public async Task<(Employee? employee, string? error)> ValidateGoogleTokenAsync(string googleToken)
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetAsync(
                $"https://oauth2.googleapis.com/tokeninfo?id_token={googleToken}");

            if (!response.IsSuccessStatusCode)
                return (null, "Invalid Google token");

            var payload = await response.Content.ReadFromJsonAsync<GoogleTokenPayload>();
            if (payload == null)
                return (null, "Failed to parse Google token");

            var employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.GoogleId == payload.Sub || e.Email == payload.Email);

            if (employee == null)
            {
                employee = new Employee
                {
                    Email = payload.Email?.ToLower() ?? string.Empty,
                    Name = payload.Name ?? payload.Email ?? string.Empty,
                    GoogleId = payload.Sub,
                    AuthProvider = "google",
                    AvatarUrl = payload.Picture
                };
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();
            }
            else if (employee.GoogleId == null)
            {
                employee.GoogleId = payload.Sub;
                employee.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            await _auditLogService.WriteAsync("Auth.Login", "Employee", employee.Id.ToString(), new
            {
                employee.Email,
                Provider = "google"
            });

            return (employee, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google token validation failed");
            return (null, "Google authentication failed");
        }
    }

    public async Task<(Employee? employee, string? error)> ValidateMicrosoftTokenAsync(string microsoftToken)
    {
        try
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", microsoftToken);

            var response = await http.GetAsync("https://graph.microsoft.com/v1.0/me");

            if (!response.IsSuccessStatusCode)
                return (null, "Invalid Microsoft token");

            var profile = await response.Content.ReadFromJsonAsync<MicrosoftProfile>();
            if (profile == null)
                return (null, "Failed to parse Microsoft profile");

            var email = profile.Mail ?? profile.UserPrincipalName ?? string.Empty;

            var employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.MicrosoftId == profile.Id || e.Email == email.ToLower());

            if (employee == null)
            {
                employee = new Employee
                {
                    Email = email.ToLower(),
                    Name = profile.DisplayName ?? email,
                    MicrosoftId = profile.Id,
                    AuthProvider = "microsoft"
                };
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();
            }
            else if (employee.MicrosoftId == null)
            {
                employee.MicrosoftId = profile.Id;
                employee.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            await _auditLogService.WriteAsync("Auth.Login", "Employee", employee.Id.ToString(), new
            {
                employee.Email,
                Provider = "microsoft"
            });

            return (employee, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Microsoft token validation failed");
            return (null, "Microsoft authentication failed");
        }
    }

    public async Task<Employee?> GetByIdAsync(Guid id) =>
        await _db.Employees.FindAsync(id);

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var employee = await _db.Employees
            .FirstOrDefaultAsync(e => e.Email == email.ToLower() && e.IsActive);

        if (employee == null) return false;

        var resetToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var ticket = new PasswordResetTicket(employee.Id, DateTime.UtcNow.Add(ResetTokenLifetime));
        ResetTickets[resetToken] = ticket;

        _logger.LogInformation("Password reset requested for: {Email}", email);
        _logger.LogDebug("Generated password reset token for {Email}: {ResetToken}", email, resetToken);
        await _auditLogService.WriteAsync("Auth.PasswordReset.Requested", "Employee", employee.Id.ToString(), new
        {
            employee.Email,
            ExpiresInMinutes = (int)ResetTokenLifetime.TotalMinutes
        });
        return true;
    }

    public async Task<(bool success, string? error)> ResetPasswordAsync(string token, string newPassword)
    {
        if (!PasswordComplexityRegex.IsMatch(newPassword))
            return (false, "Password policy requirements were not met.");

        if (!ResetTickets.TryGetValue(token, out var ticket))
            return (false, "Invalid or expired reset token.");

        if (ticket.ExpiresAtUtc <= DateTime.UtcNow)
        {
            ResetTickets.TryRemove(token, out _);
            return (false, "Invalid or expired reset token.");
        }

        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == ticket.EmployeeId && e.IsActive);
        if (employee == null)
        {
            ResetTickets.TryRemove(token, out _);
            return (false, "Reset request could not be completed.");
        }

        employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        employee.AuthProvider = "email";
        employee.FailedLoginCount = 0;
        employee.LockoutEndUtc = null;
        employee.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        ResetTickets.TryRemove(token, out _);

        await _auditLogService.WriteAsync("Auth.PasswordReset.Completed", "Employee", employee.Id.ToString(), new
        {
            employee.Email
        });

        return (true, null);
    }
}

internal sealed record PasswordResetTicket(Guid EmployeeId, DateTime ExpiresAtUtc);

internal class GoogleTokenPayload
{
    public string? Sub { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Picture { get; set; }
}

internal class MicrosoftProfile
{
    public string? Id { get; set; }
    public string? DisplayName { get; set; }
    public string? Mail { get; set; }
    public string? UserPrincipalName { get; set; }
}
