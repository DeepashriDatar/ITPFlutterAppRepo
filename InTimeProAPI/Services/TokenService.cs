using InTimeProAPI.Data;
using InTimeProAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InTimeProAPI.Services;

public class TokenService : ITokenService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public TokenService(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public string GenerateAccessToken(Employee employee)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]!;
        var expiryMinutes = int.Parse(jwtSettings["AccessTokenExpiryMinutes"] ?? "60");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, employee.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, employee.Email),
            new Claim(JwtRegisteredClaimNames.Name, employee.Name),
            new Claim("department", employee.Department ?? ""),
            new Claim("role", employee.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public async Task<RefreshToken> SaveRefreshTokenAsync(
        Guid employeeId, string token, bool rememberMe)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var expiryDays = rememberMe
            ? int.Parse(jwtSettings["RefreshTokenExpiryDays"] ?? "30")
            : 1; // 1 day if not "remember me"

        var refreshToken = new RefreshToken
        {
            EmployeeId = employeeId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(expiryDays)
        };

        // Revoke old tokens for this user
        var oldTokens = await _db.RefreshTokens
            .Where(rt => rt.EmployeeId == employeeId && !rt.IsRevoked)
            .ToListAsync();
        oldTokens.ForEach(rt => rt.IsRevoked = true);

        _db.RefreshTokens.Add(refreshToken);
        await _db.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<(Employee? employee, string? error)> ValidateRefreshTokenAsync(
        string refreshToken)
    {
        var token = await _db.RefreshTokens
            .Include(rt => rt.Employee)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (token == null)
            return (null, "Invalid refresh token");

        if (!token.IsActive)
            return (null, token.IsExpired ? "Refresh token expired" : "Refresh token revoked");

        return (token.Employee, null);
    }

    public async Task RevokeRefreshTokenAsync(Guid employeeId)
    {
        var tokens = await _db.RefreshTokens
            .Where(rt => rt.EmployeeId == employeeId && !rt.IsRevoked)
            .ToListAsync();
        tokens.ForEach(rt => rt.IsRevoked = true);
        await _db.SaveChangesAsync();
    }
}
