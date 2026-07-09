using System.ComponentModel.DataAnnotations;

namespace BlossomTreeManager.Web.Data.Entities;

public class ParentStudentLink
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public string ParentUserId { get; set; } = string.Empty;
    public Guid StudentId { get; set; }

    [MaxLength(100)]
    public string? Relationship { get; set; }

    public bool IsPrimary { get; set; }

    public School School { get; set; } = null!;
    public ApplicationUser ParentUser { get; set; } = null!;
    public StudentProfile Student { get; set; } = null!;
}
