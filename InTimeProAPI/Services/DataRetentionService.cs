using InTimeProAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace InTimeProAPI.Services;

public class DataRetentionService : IDataRetentionService
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataRetentionService> _logger;

    public DataRetentionService(
        AppDbContext dbContext,
        IConfiguration configuration,
        ILogger<DataRetentionService> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task ApplyRetentionAsync(CancellationToken cancellationToken)
    {
        var auditMonths = _configuration.GetValue<int?>("DataRetention:EmployeeAuditMonths") ?? 24;
        var piiMonths = _configuration.GetValue<int?>("DataRetention:EmployeePiiRetentionMonths") ?? 36;
        var deleteEnabled = _configuration.GetValue<bool?>("DataRetention:DeleteEnabled") ?? false;

        var auditThreshold = DateTime.UtcNow.AddMonths(-auditMonths);

        var staleCount = await _dbContext.AuditLogEntries
            .Where(entry => entry.OccurredAtUtc < auditThreshold)
            .CountAsync(cancellationToken);

        if (deleteEnabled && staleCount > 0)
        {
            var deleted = await _dbContext.AuditLogEntries
                .Where(entry => entry.OccurredAtUtc < auditThreshold)
                .ExecuteDeleteAsync(cancellationToken);

            _logger.LogWarning("Data retention deletion executed. Deleted {Deleted} audit rows older than {Months} months", deleted, auditMonths);
        }
        else
        {
            _logger.LogInformation("Data retention dry-run complete. Stale audit rows older than {Months} months: {Count}", auditMonths, staleCount);
        }

        _logger.LogInformation("Configured employee PII retention window: {PiiMonths} months", piiMonths);
    }
}
