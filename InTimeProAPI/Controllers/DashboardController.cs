using System.Security.Claims;
using InTimeProAPI.DTOs;
using InTimeProAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InTimeProAPI.Controllers;

[ApiController]
[Route("api/v1/dashboard")]
[Authorize]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetSummary(CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
        {
            return Unauthorized(ApiResponse<DashboardSummaryDto>.Fail("Invalid token"));
        }

        var summary = await _dashboardService.GetSummaryAsync(employeeId, cancellationToken);
        return Ok(ApiResponse<DashboardSummaryDto>.Ok(summary));
    }

    [HttpPost("active-task/actions/{action}")]
    public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> ApplyAction(
        [FromRoute] string action,
        CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
        {
            return Unauthorized(ApiResponse<DashboardSummaryDto>.Fail("Invalid token"));
        }

        var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "start",
            "pause",
            "complete"
        };

        if (!allowed.Contains(action))
        {
            return BadRequest(ApiResponse<DashboardSummaryDto>.Fail("Unsupported action"));
        }

        var summary = await _dashboardService.ApplyTaskActionAsync(employeeId, action, cancellationToken);
        return Ok(ApiResponse<DashboardSummaryDto>.Ok(summary));
    }

    private bool TryGetEmployeeId(out Guid employeeId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        return Guid.TryParse(userIdClaim, out employeeId);
    }
}
