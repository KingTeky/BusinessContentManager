using Microsoft.AspNetCore.Identity;
using BlossomTreeManager.Web.Data.Entities;

namespace BlossomTreeManager.Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public Guid? SchoolId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public School? School { get; set; }
    public ICollection<RoomTeacherAssignment> RoomAssignments { get; set; } = [];
    public ICollection<ParentStudentLink> ParentLinks { get; set; } = [];
    public ICollection<AttendanceRecord> MarkedAttendanceRecords { get; set; } = [];
    public ICollection<NotificationMessage> SentNotifications { get; set; } = [];
    public ICollection<NotificationMessage> ReceivedNotifications { get; set; } = [];
}

