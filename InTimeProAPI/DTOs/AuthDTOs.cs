using System.ComponentModel.DataAnnotations;

namespace InTimeProAPI.DTOs;

// ── Request DTOs ──────────────────────────────────────────────────────────────

public class LoginRequest
    : IValidatableObject
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Password { get; set; }

    /// <summary>Provider: "email" | "google" | "microsoft"</summary>
    [Required]
    public string Provider { get; set; } = "email";

    /// <summary>OAuth token from Google/Microsoft SDK (required for social login)</summary>
    public string? SocialToken { get; set; }

    public bool RememberMe { get; set; } = false;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var provider = Provider.Trim().ToLowerInvariant();
        var validProvider = provider is "email" or "google" or "microsoft";
        if (!validProvider)
        {
            yield return new ValidationResult(
                "Provider must be one of email, google, or microsoft.",
                new[] { nameof(Provider) });
        }

        if (provider == "email" && string.IsNullOrWhiteSpace(Password))
        {
            yield return new ValidationResult(
                "Password is required for email login.",
                new[] { nameof(Password) });
        }

        if ((provider == "google" || provider == "microsoft") && string.IsNullOrWhiteSpace(SocialToken))
        {
            yield return new ValidationResult(
                "SocialToken is required for social login.",
                new[] { nameof(SocialToken) });
        }
    }
}

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class ForgotPasswordRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,}$",
        ErrorMessage = "Password must be at least 8 characters and include upper, lower, number, and special character.")]
    public string NewPassword { get; set; } = string.Empty;
}

// ── Response DTOs ─────────────────────────────────────────────────────────────

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors };
}

public class LoginResponse
{
    public UserDto User { get; set; } = null!;
    public TokenDto Tokens { get; set; } = null!;
}

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; } // seconds
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
}

public class RefreshResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}
