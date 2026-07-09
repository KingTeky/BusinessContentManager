using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlossomTreeManager.Web.Data.Constants;
using BlossomTreeManager.Web.Data.Entities;
using BlossomTreeManager.Web.Data.Enums;

namespace BlossomTreeManager.Web.Data.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await dbContext.Database.MigrateAsync();

        foreach (var role in ApplicationRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var school = await dbContext.Schools.FirstOrDefaultAsync(x => x.Name == "Sunnydale Elementary");
        if (school is null)
        {
            school = new School
            {
                Id = Guid.NewGuid(),
                Name = "Sunnydale Elementary",
                Address = "123 Education Street, Sunnydale, CA 90210",
                Timezone = "America/Los_Angeles",
                IsActive = true
            };
            dbContext.Schools.Add(school);
            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.SchoolSettings.AnyAsync(x => x.SchoolId == school.Id))
        {
            dbContext.SchoolSettings.Add(new SchoolSetting
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                DefaultCheckInTime = new TimeOnly(8, 0),
                DefaultCheckOutTime = new TimeOnly(15, 0),
                NotificationEmail = "admin@sunnydale.edu",
                PrimaryTimezone = "America/Los_Angeles"
            });
            await dbContext.SaveChangesAsync();
        }

        var appOwner = await EnsureUserAsync(userManager, "owner@blossomtree.app", "Blossom Owner", null, ApplicationRoles.AppOwner);
        var schoolAdmin = await EnsureUserAsync(userManager, "principal@sunnydale.edu", "Mike Principal", school.Id, ApplicationRoles.SchoolAdmin);
        var teacher = await EnsureUserAsync(userManager, "johnson@sunnydale.edu", "Ms. Johnson", school.Id, ApplicationRoles.Teacher);
        var parent = await EnsureUserAsync(userManager, "john.parent@email.com", "John Parent", school.Id, ApplicationRoles.Parent);

        if (!await dbContext.Rooms.AnyAsync(x => x.SchoolId == school.Id))
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                Name = "Room 101 - Kindergarten",
                GradeLevel = "Kindergarten",
                Capacity = 20
            };

            var student = new StudentProfile
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                FirstName = "Emma",
                LastName = "Johnson",
                DateOfBirth = new DateOnly(2020, 2, 7),
                IsActive = true
            };

            dbContext.Rooms.Add(room);
            dbContext.Students.Add(student);
            await dbContext.SaveChangesAsync();

            dbContext.RoomTeacherAssignments.Add(new RoomTeacherAssignment
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                RoomId = room.Id,
                TeacherUserId = teacher.Id,
                IsPrimary = true
            });

            dbContext.ParentStudentLinks.Add(new ParentStudentLink
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                ParentUserId = parent.Id,
                StudentId = student.Id,
                Relationship = "Parent",
                IsPrimary = true
            });

            dbContext.StudentEnrollments.Add(new StudentEnrollment
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                RoomId = room.Id,
                StudentId = student.Id,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.Date),
                IsActive = true
            });

            dbContext.AttendanceRecords.Add(new AttendanceRecord
            {
                Id = Guid.NewGuid(),
                SchoolId = school.Id,
                StudentId = student.Id,
                RoomId = room.Id,
                AttendanceDate = DateOnly.FromDateTime(DateTime.UtcNow.Date),
                CheckInAtUtc = DateTime.UtcNow.Date.AddHours(15),
                Status = AttendanceStatus.Present,
                MarkedByUserId = schoolAdmin.Id
            });

            await dbContext.SaveChangesAsync();
        }

        _ = appOwner;
    }

    private static async Task<ApplicationUser> EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string fullName,
        Guid? schoolId,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName,
                SchoolId = schoolId,
                IsActive = true
            };

            var createResult = await userManager.CreateAsync(user, "ChangeMe123!");
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException($"Could not create user {email}: {string.Join(", ", createResult.Errors.Select(x => x.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }

        return user;
    }
}
