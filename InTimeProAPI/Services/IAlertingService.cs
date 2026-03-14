namespace InTimeProAPI.Services;

public interface IAlertingService
{
    Task AlertFailedLoginAsync(string email, string reason, CancellationToken cancellationToken = default);
    Task AlertOverdueTaskAsync(Guid taskId, Guid employeeId, CancellationToken cancellationToken = default);
    Task AlertTimesheetSubmissionFailedAsync(Guid employeeId, DateOnly date, string reason, CancellationToken cancellationToken = default);
}
