using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlossomTreeManager.Web.Data.Entities;

namespace BlossomTreeManager.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<School> Schools => Set<School>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<StudentProfile> Students => Set<StudentProfile>();
    public DbSet<ParentStudentLink> ParentStudentLinks => Set<ParentStudentLink>();
    public DbSet<RoomTeacherAssignment> RoomTeacherAssignments => Set<RoomTeacherAssignment>();
    public DbSet<StudentEnrollment> StudentEnrollments => Set<StudentEnrollment>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<SchoolSetting> SchoolSettings => Set<SchoolSetting>();
    public DbSet<NotificationMessage> Notifications => Set<NotificationMessage>();
    public DbSet<AuditLogEntry> AuditLogs => Set<AuditLogEntry>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasOne(x => x.School)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<School>()
            .HasIndex(x => x.Name);

        builder.Entity<Room>()
            .HasIndex(x => new { x.SchoolId, x.Name })
            .IsUnique();

        builder.Entity<StudentProfile>()
            .HasIndex(x => new { x.SchoolId, x.LastName, x.FirstName });

        builder.Entity<ParentStudentLink>()
            .HasIndex(x => new { x.ParentUserId, x.StudentId })
            .IsUnique();

        builder.Entity<ParentStudentLink>()
            .HasOne(x => x.ParentUser)
            .WithMany(x => x.ParentLinks)
            .HasForeignKey(x => x.ParentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<RoomTeacherAssignment>()
            .HasIndex(x => new { x.RoomId, x.TeacherUserId })
            .IsUnique();

        builder.Entity<RoomTeacherAssignment>()
            .HasOne(x => x.TeacherUser)
            .WithMany(x => x.RoomAssignments)
            .HasForeignKey(x => x.TeacherUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<StudentEnrollment>()
            .HasIndex(x => new { x.SchoolId, x.StudentId, x.IsActive });

        builder.Entity<AttendanceRecord>()
            .HasIndex(x => new { x.StudentId, x.AttendanceDate })
            .IsUnique();

        builder.Entity<AttendanceRecord>()
            .HasOne(x => x.MarkedByUser)
            .WithMany(x => x.MarkedAttendanceRecords)
            .HasForeignKey(x => x.MarkedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<SchoolSetting>()
            .HasIndex(x => x.SchoolId)
            .IsUnique();

        builder.Entity<NotificationMessage>()
            .HasOne(x => x.SenderUser)
            .WithMany(x => x.SentNotifications)
            .HasForeignKey(x => x.SenderUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<NotificationMessage>()
            .HasOne(x => x.ReceiverUser)
            .WithMany(x => x.ReceivedNotifications)
            .HasForeignKey(x => x.ReceiverUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AuditLogEntry>()
            .HasOne(x => x.ActorUser)
            .WithMany()
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
