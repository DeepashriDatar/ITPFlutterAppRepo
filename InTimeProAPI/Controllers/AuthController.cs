using InTimeProAPI.DTOs;
using InTimeProAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InTimeProAPI.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        ITokenService tokenService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>Login with email/password or social provider</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<LoginResponse>.Fail("Validation failed",
                ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));

        (var employee, var error) = request.Provider switch
        {
            "google" when !string.IsNullOrEmpty(request.SocialToken) =>
                await _authService.ValidateGoogleTokenAsync(request.SocialToken),
            "microsoft" when !string.IsNullOrEmpty(request.SocialToken) =>
                await _authService.ValidateMicrosoftTokenAsync(request.SocialToken),
            "email" =>
                await _authService.ValidateEmailPasswordAsync(request.Email, request.Password ?? ""),
            _ => (null, "Invalid provider or missing token")
        };

        if (employee == null)
            return Unauthorized(ApiResponse<LoginResponse>.Fail(error ?? "Authentication failed"));

        var accessToken = _tokenService.GenerateAccessToken(employee);
        var refreshToken = _tokenService.GenerateRefreshToken();
        await _tokenService.SaveRefreshTokenAsync(employee.Id, refreshToken, request.RememberMe);

        var jwtExpiry = int.Parse(
            HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()
                .GetSection("JwtSettings")["AccessTokenExpiryMinutes"] ?? "60");

        _logger.LogInformation("User {Email} logged in via {Provider}", employee.Email, request.Provider);

        return Ok(ApiResponse<LoginResponse>.Ok(new LoginResponse
        {
            User = new UserDto
            {
                Id = employee.Id,
                Email = employee.Email,
                Name = employee.Name,
                Department = employee.Department,
                Role = employee.Role,
                Phone = employee.Phone,
                AvatarUrl = employee.AvatarUrl
            },
            Tokens = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = jwtExpiry * 60
            }
        }));
    }

    /// <summary>Refresh access token using refresh token</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<RefreshResponse>>> Refresh(
        [FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<RefreshResponse>.Fail("Validation failed"));

        var (employee, error) = await _tokenService.ValidateRefreshTokenAsync(request.RefreshToken);
        if (employee == null)
            return Unauthorized(ApiResponse<RefreshResponse>.Fail(error ?? "Invalid refresh token"));

        var newAccessToken = _tokenService.GenerateAccessToken(employee);
        var jwtExpiry = int.Parse(
            HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()
                .GetSection("JwtSettings")["AccessTokenExpiryMinutes"] ?? "60");

        return Ok(ApiResponse<RefreshResponse>.Ok(new RefreshResponse
        {
            AccessToken = newAccessToken,
            ExpiresIn = jwtExpiry * 60
        }));
    }

    /// <summary>Logout and revoke all refresh tokens</summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (Guid.TryParse(userIdClaim, out var userId))
            await _tokenService.RevokeRefreshTokenAsync(userId);

        return Ok(ApiResponse<object>.Ok(null, "Logged out successfully"));
    }

    /// <summary>Request password reset email</summary>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> ForgotPassword(
        [FromBody] ForgotPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation failed"));

        // Always return success to prevent email enumeration attacks
        await _authService.ForgotPasswordAsync(request.Email);

        return Ok(ApiResponse<object>.Ok(null,
            "If this email is registered, a password reset link has been sent."));
    }

    /// <summary>Get current authenticated user profile</summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetMe()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(ApiResponse<UserDto>.Fail("Invalid token"));

        var employee = await _authService.GetByIdAsync(userId);
        if (employee == null)
            return NotFound(ApiResponse<UserDto>.Fail("User not found"));

        return Ok(ApiResponse<UserDto>.Ok(new UserDto
        {
            Id = employee.Id,
            Email = employee.Email,
            Name = employee.Name,
            Department = employee.Department,
            Role = employee.Role,
            Phone = employee.Phone,
            AvatarUrl = employee.AvatarUrl
        }));
    }
}
