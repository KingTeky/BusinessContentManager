using Microsoft.EntityFrameworkCore;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Enums;

namespace BlossomTreeManager.Web.Services;

public record DashboardSummary(
    int TotalStudents,
    int TotalTeachers,
    int ActiveParents,
    int TodaysCheckIns);

public class DashboardService(ApplicationDbContext dbContext)
{
    public async Task<DashboardSummary> GetSummaryAsync(Guid schoolId, DateOnly today)
    {
        var totalStudents = await dbContext.Students
            .Where(x => x.SchoolId == schoolId && x.IsActive)
            .CountAsync();

        var totalTeachers = await dbContext.Users
            .Where(x => x.SchoolId == schoolId)
            .Join(dbContext.UserRoles,
                user => user.Id,
                role => role.UserId,
                (user, role) => new { user, role })
            .Join(dbContext.Roles,
                x => x.role.RoleId,
                role => role.Id,
                (x, role) => new { x.user, RoleName = role.Name })
            .Where(x => x.RoleName == "Teacher")
            .Select(x => x.user.Id)
            .Distinct()
            .CountAsync();

        var activeParents = await dbContext.Users
            .Where(x => x.SchoolId == schoolId)
            .Join(dbContext.UserRoles,
                user => user.Id,
                role => role.UserId,
                (user, role) => new { user, role })
            .Join(dbContext.Roles,
                x => x.role.RoleId,
                role => role.Id,
                (x, role) => new { x.user, RoleName = role.Name })
            .Where(x => x.RoleName == "Parent")
            .Select(x => x.user.Id)
            .Distinct()
            .CountAsync();

        var todaysCheckIns = await dbContext.AttendanceRecords
            .Where(x => x.SchoolId == schoolId && x.AttendanceDate == today && x.Status != AttendanceStatus.Absent)
            .CountAsync();

        return new DashboardSummary(totalStudents, totalTeachers, activeParents, todaysCheckIns);
    }
}
