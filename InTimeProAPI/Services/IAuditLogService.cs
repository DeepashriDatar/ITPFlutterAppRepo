namespace InTimeProAPI.Services;

public interface IAuditLogService
{
    Task WriteAsync(
        string action,
        string entityType,
        string entityId,
        object? metadata = null,
        CancellationToken cancellationToken = default);
}
