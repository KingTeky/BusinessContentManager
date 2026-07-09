namespace BlossomTreeManager.Web.Data.Entities;

public class RoomTeacherAssignment
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid RoomId { get; set; }
    public string TeacherUserId { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    public School School { get; set; } = null!;
    public Room Room { get; set; } = null!;
    public ApplicationUser TeacherUser { get; set; } = null!;
}
