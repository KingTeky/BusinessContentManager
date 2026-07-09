using System.ComponentModel.DataAnnotations;

namespace BlossomTreeManager.Web.Data.Entities;

public class AuditLogEntry
{
    public Guid Id { get; set; }
    public Guid? SchoolId { get; set; }
    public string? ActorUserId { get; set; }

    [MaxLength(150)]
    public required string Action { get; set; }

    [MaxLength(150)]
    public required string EntityName { get; set; }

    [MaxLength(100)]
    public required string EntityId { get; set; }

    [MaxLength(3000)]
    public string? MetadataJson { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public School? School { get; set; }
    public ApplicationUser? ActorUser { get; set; }
}
