using BlossomTreeManager.Web.Data.Enums;

namespace BlossomTreeManager.Web.Data.Entities;

public class AttendanceRecord
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentId { get; set; }
    public Guid RoomId { get; set; }
    public DateOnly AttendanceDate { get; set; }
    public DateTime? CheckInAtUtc { get; set; }
    public DateTime? CheckOutAtUtc { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? MarkedByUserId { get; set; }

    public School School { get; set; } = null!;
    public StudentProfile Student { get; set; } = null!;
    public Room Room { get; set; } = null!;
    public ApplicationUser? MarkedByUser { get; set; }
}
