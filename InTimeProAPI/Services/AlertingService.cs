namespace InTimeProAPI.Services;

public class AlertingService : IAlertingService
{
    private readonly ILogger<AlertingService> _logger;

    public AlertingService(ILogger<AlertingService> logger)
    {
        _logger = logger;
    }

    public Task AlertFailedLoginAsync(string email, string reason, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("ALERT failed-login email={Email} reason={Reason}", email, reason);
        return Task.CompletedTask;
    }

    public Task AlertOverdueTaskAsync(Guid taskId, Guid employeeId, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("ALERT overdue-task taskId={TaskId} employeeId={EmployeeId}", taskId, employeeId);
        return Task.CompletedTask;
    }

    public Task AlertTimesheetSubmissionFailedAsync(Guid employeeId, DateOnly date, string reason, CancellationToken cancellationToken = default)
    {
        _logger.LogError("ALERT timesheet-submit-failed employeeId={EmployeeId} date={Date} reason={Reason}", employeeId, date, reason);
        return Task.CompletedTask;
    }
}
