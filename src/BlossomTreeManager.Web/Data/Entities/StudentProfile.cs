using System.ComponentModel.DataAnnotations;

namespace BlossomTreeManager.Web.Data.Entities;

public class StudentProfile
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }

    [MaxLength(100)]
    public required string FirstName { get; set; }

    [MaxLength(100)]
    public required string LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }
    public bool IsActive { get; set; } = true;

    public School School { get; set; } = null!;
    public ICollection<ParentStudentLink> ParentLinks { get; set; } = [];
    public ICollection<StudentEnrollment> Enrollments { get; set; } = [];
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = [];
}
