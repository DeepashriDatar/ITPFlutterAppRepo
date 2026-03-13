using InTimeProAPI.Data;
using InTimeProAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InTimeProAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext db, ILogger<AuthService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<(Employee? employee, string? error)> ValidateEmailPasswordAsync(
        string email, string password)
    {
        var employee = await _db.Employees
            .FirstOrDefaultAsync(e => e.Email == email.ToLower() && e.IsActive);

        if (employee == null)
            return (null, "Invalid email or password");

        if (employee.PasswordHash == null)
            return (null, "This account uses social login. Please use Google or Microsoft to sign in.");

        bool passwordValid = BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash);
        if (!passwordValid)
            return (null, "Invalid email or password");

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

        _logger.LogInformation("Password reset requested for: {Email}", email);
        return true;
    }
}

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
