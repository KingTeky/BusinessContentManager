namespace BlossomTreeManager.Web.Data.Entities;

public class StudentEnrollment
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentId { get; set; }
    public Guid RoomId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public School School { get; set; } = null!;
    public StudentProfile Student { get; set; } = null!;
    public Room Room { get; set; } = null!;
}
