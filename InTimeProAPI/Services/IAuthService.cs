using InTimeProAPI.DTOs;
using InTimeProAPI.Models;

namespace InTimeProAPI.Services;

public interface IAuthService
{
    Task<(Employee? employee, string? error)> ValidateEmailPasswordAsync(string email, string password);
    Task<(Employee? employee, string? error)> ValidateGoogleTokenAsync(string googleToken);
    Task<(Employee? employee, string? error)> ValidateMicrosoftTokenAsync(string microsoftToken);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<bool> ForgotPasswordAsync(string email);
    Task<(bool success, string? error)> ResetPasswordAsync(string token, string newPassword);
}

public interface ITokenService
{
    string GenerateAccessToken(Employee employee);
    string GenerateRefreshToken();
    Task<RefreshToken> SaveRefreshTokenAsync(Guid employeeId, string token, bool rememberMe);
    Task<(Employee? employee, string? error)> ValidateRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(Guid employeeId);
}
