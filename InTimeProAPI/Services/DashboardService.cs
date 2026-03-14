using System.Collections.Concurrent;
using InTimeProAPI.DTOs;

namespace InTimeProAPI.Services;

public class DashboardService
{
    private readonly IAuditLogService _auditLogService;

    private static readonly ConcurrentDictionary<Guid, DashboardSummaryDto> _summaries = new();

    public DashboardService(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public Task<DashboardSummaryDto> GetSummaryAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        var summary = _summaries.GetOrAdd(employeeId, _ => new DashboardSummaryDto
        {
            ClockInTime = DateTime.UtcNow.AddHours(-2).ToString("HH:mm"),
            ActiveWorkingHours = "2h 00m",
            TotalWorkTime = "2h 00m",
            ActiveTaskName = "Daily Standup Follow-ups",
            ActiveTaskState = "In Progress",
            ActiveTaskElapsedSeconds = 7200
        });

        return Task.FromResult(summary);
    }

    public async Task<DashboardSummaryDto> ApplyTaskActionAsync(
        Guid employeeId,
        string action,
        CancellationToken cancellationToken = default)
    {
        var existing = await GetSummaryAsync(employeeId, cancellationToken);
        DashboardSummaryDto updated = action.ToLowerInvariant() switch
        {
            "start" => existing with { ActiveTaskState = "In Progress" },
            "pause" => existing with { ActiveTaskState = "Paused" },
            "complete" => existing with { ActiveTaskState = "Completed" },
            _ => existing
        };

        _summaries[employeeId] = updated;

        await _auditLogService.WriteAsync(
            action: $"dashboard.task.{action.ToLowerInvariant()}",
            entityType: "DashboardActiveTask",
            entityId: employeeId.ToString(),
            metadata: new { action },
            cancellationToken: cancellationToken);

        return updated;
    }
}
