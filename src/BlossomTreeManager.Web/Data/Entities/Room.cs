using System.ComponentModel.DataAnnotations;

namespace BlossomTreeManager.Web.Data.Entities;

public class Room
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(100)]
    public required string GradeLevel { get; set; }

    public int Capacity { get; set; }

    public School School { get; set; } = null!;
    public ICollection<RoomTeacherAssignment> TeacherAssignments { get; set; } = [];
    public ICollection<StudentEnrollment> StudentEnrollments { get; set; } = [];
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];
}
