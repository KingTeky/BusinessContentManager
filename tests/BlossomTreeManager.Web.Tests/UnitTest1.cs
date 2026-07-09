using Microsoft.EntityFrameworkCore;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Services;

namespace BlossomTreeManager.Web.Tests;

public class TenantGuardServiceTests
{
    [Fact]
    public async Task RequireSchoolMutationAccessAsync_Allows_GlobalUser()
    {
        await using var dbContext = CreateDbContext();
        var guard = new TenantGuardService(
            new FakeSchoolContext(Guid.NewGuid(), isGlobalUser: true, userId: "user-1"),
            dbContext);

        var exception = await Record.ExceptionAsync(() => guard.RequireSchoolMutationAccessAsync(
            Guid.NewGuid(),
            action: "Room:Update",
            entityName: "Room",
            entityId: Guid.NewGuid().ToString()));

        Assert.Null(exception);
        Assert.Empty(dbContext.AuditLogs);
    }

    [Fact]
    public async Task RequireSchoolMutationAccessAsync_Allows_SameSchoolUser()
    {
        var schoolId = Guid.NewGuid();

        await using var dbContext = CreateDbContext();
        var guard = new TenantGuardService(
            new FakeSchoolContext(schoolId, isGlobalUser: false, userId: "user-2"),
            dbContext);

        var exception = await Record.ExceptionAsync(() => guard.RequireSchoolMutationAccessAsync(
            schoolId,
            action: "Student:Update",
            entityName: "StudentProfile",
            entityId: Guid.NewGuid().ToString()));

        Assert.Null(exception);
        Assert.Empty(dbContext.AuditLogs);
    }

    [Fact]
    public async Task RequireSchoolMutationAccessAsync_Denies_CrossSchoolAndWritesAudit()
    {
        var actorSchoolId = Guid.NewGuid();
        var targetSchoolId = Guid.NewGuid();

        await using var dbContext = CreateDbContext();
        var guard = new TenantGuardService(
            new FakeSchoolContext(actorSchoolId, isGlobalUser: false, userId: "user-3"),
            dbContext);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => guard.RequireSchoolMutationAccessAsync(
            targetSchoolId,
            action: "Attendance:CheckOut",
            entityName: "AttendanceRecord",
            entityId: "attendance-1",
            deniedMetadata: new { Reason = "CrossSchool" }));

        var entry = await dbContext.AuditLogs.SingleAsync();
        Assert.Equal("DENIED:Attendance:CheckOut", entry.Action);
        Assert.Equal("AttendanceRecord", entry.EntityName);
        Assert.Equal("attendance-1", entry.EntityId);
        Assert.Equal(targetSchoolId, entry.SchoolId);
        Assert.Equal("user-3", entry.ActorUserId);
        Assert.NotNull(entry.MetadataJson);
        Assert.Contains("CrossSchool", entry.MetadataJson);
    }

    [Fact]
    public async Task LogMutationAsync_Writes_AuditEntry()
    {
        var schoolId = Guid.NewGuid();

        await using var dbContext = CreateDbContext();
        var guard = new TenantGuardService(
            new FakeSchoolContext(schoolId, isGlobalUser: false, userId: "user-4"),
            dbContext);

        await guard.LogMutationAsync(
            action: "User:StatusChange",
            entityName: "ApplicationUser",
            entityId: "user-5",
            schoolId: schoolId,
            metadata: new { PreviousStatus = true, NewStatus = false });

        var entry = await dbContext.AuditLogs.SingleAsync();
        Assert.Equal("User:StatusChange", entry.Action);
        Assert.Equal("ApplicationUser", entry.EntityName);
        Assert.Equal("user-5", entry.EntityId);
        Assert.Equal(schoolId, entry.SchoolId);
        Assert.Equal("user-4", entry.ActorUserId);
        Assert.NotNull(entry.MetadataJson);
        Assert.Contains("PreviousStatus", entry.MetadataJson);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private sealed class FakeSchoolContext(Guid? schoolId, bool isGlobalUser, string? userId) : ISchoolContext
    {
        public Task<Guid?> GetSchoolIdAsync() => Task.FromResult(schoolId);

        public Task<bool> IsGlobalUserAsync() => Task.FromResult(isGlobalUser);

        public Task<string?> GetUserIdAsync() => Task.FromResult(userId);
    }
}
