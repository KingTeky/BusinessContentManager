using System.ComponentModel.DataAnnotations;

namespace BlossomTreeManager.Web.Data.Entities;

public class SchoolSetting
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }

    public TimeOnly DefaultCheckInTime { get; set; } = new(8, 0);
    public TimeOnly DefaultCheckOutTime { get; set; } = new(15, 0);

    [MaxLength(256)]
    public string? NotificationEmail { get; set; }

    [MaxLength(100)]
    public string PrimaryTimezone { get; set; } = "America/Los_Angeles";

    public School School { get; set; } = null!;
}
