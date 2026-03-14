using System.ComponentModel.DataAnnotations;

namespace InTimeProAPI.Models;

public class AuditLogEntry
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(120)]
    public string Action { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string EntityType { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string EntityId { get; set; } = string.Empty;

    [MaxLength(120)]
    public string? ActorId { get; set; }

    [MaxLength(200)]
    public string? ActorEmail { get; set; }

    [MaxLength(100)]
    public string? CorrelationId { get; set; }

    [MaxLength(80)]
    public string? SourceIp { get; set; }

    public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;

    public string? MetadataJson { get; set; }
}
