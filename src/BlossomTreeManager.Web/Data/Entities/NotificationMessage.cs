using System.ComponentModel.DataAnnotations;
using BlossomTreeManager.Web.Data.Enums;

namespace BlossomTreeManager.Web.Data.Entities;

public class NotificationMessage
{
    public Guid Id { get; set; }
    public Guid? SchoolId { get; set; }
    public string? SenderUserId { get; set; }
    public string ReceiverUserId { get; set; } = string.Empty;

    [MaxLength(1000)]
    public required string Message { get; set; }

    public NotificationChannel Channel { get; set; }
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? SentAtUtc { get; set; }

    public School? School { get; set; }
    public ApplicationUser? SenderUser { get; set; }
    public ApplicationUser ReceiverUser { get; set; } = null!;
}
