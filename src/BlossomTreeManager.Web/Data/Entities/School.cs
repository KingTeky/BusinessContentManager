using System.ComponentModel.DataAnnotations;

namespace BlossomTreeManager.Web.Data.Entities;

public class School
{
    public Guid Id { get; set; }

    [MaxLength(150)]
    public required string Name { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string Timezone { get; set; } = "America/Los_Angeles";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<ApplicationUser> Users { get; set; } = [];
    public ICollection<Room> Rooms { get; set; } = [];
    public ICollection<StudentProfile> Students { get; set; } = [];
}
