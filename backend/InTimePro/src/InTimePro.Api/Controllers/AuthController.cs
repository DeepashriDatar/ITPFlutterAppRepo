using InTimePro.Api.Dtos;
using InTimePro.Api.Infrastructure;
using InTimePro.Api.Services;
using System.Web.Http;

namespace InTimePro.Api.Controllers
{
    [RoutePrefix("api/v1/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController()
        {
            _authService = new AuthService();
        }

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] AuthDto request)
        {
            var result = _authService.Login(request);
            if (result == null)
            {
                return Content(System.Net.HttpStatusCode.Unauthorized,
                    ApiResponse<object>.Fail("Invalid credentials or provider."));
            }

            return Ok(ApiResponse<AuthResultDto>.Ok(result));
        }

        [HttpPost]
        [Route("refresh")]
        public IHttpActionResult Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var result = _authService.Refresh(request?.RefreshToken);
            if (result == null)
            {
                return Content(System.Net.HttpStatusCode.Unauthorized,
                    ApiResponse<object>.Fail("Invalid or expired refresh token."));
            }

            return Ok(ApiResponse<RefreshResultDto>.Ok(result));
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout([FromBody] LogoutDto request)
        {
            var ok = _authService.Logout(request?.RefreshToken);
            if (!ok)
            {
                return BadRequest("Refresh token is required and must be valid.");
            }

            return Ok(ApiResponse<object>.Ok(null, "Logged out successfully"));
        }
    }
}
