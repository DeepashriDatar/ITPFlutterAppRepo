using System.Security.Claims;
using System.Text.Json;
using InTimeProAPI.Data;
using InTimeProAPI.Middleware;
using InTimeProAPI.Models;

namespace InTimeProAPI.Services;

public class AuditLogService : IAuditLogService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuditLogService> _logger;

    public AuditLogService(
        AppDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuditLogService> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task WriteAsync(
        string action,
        string entityType,
        string entityId,
        object? metadata = null,
        CancellationToken cancellationToken = default)
    {
        var context = _httpContextAccessor.HttpContext;
        var actorId = context?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? context?.User.FindFirstValue("sub");
        var actorEmail = context?.User.FindFirstValue(ClaimTypes.Email)
                         ?? context?.User.FindFirstValue("email");
        var correlationId = context?.Request.Headers[CorrelationIdMiddleware.HeaderName].FirstOrDefault()
                            ?? context?.Items[CorrelationIdMiddleware.HeaderName]?.ToString();

        var entry = new AuditLogEntry
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            ActorId = actorId,
            ActorEmail = actorEmail,
            CorrelationId = correlationId,
            SourceIp = context?.Connection.RemoteIpAddress?.ToString(),
            MetadataJson = metadata == null ? null : JsonSerializer.Serialize(metadata)
        };

        _dbContext.AuditLogEntries.Add(entry);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AUDIT {Action} {EntityType}:{EntityId}", action, entityType, entityId);
    }
}
