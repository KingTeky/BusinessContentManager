using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Constants;
using BlossomTreeManager.Web.Services;

namespace BlossomTreeManager.Web.Tests;

public class UserManagementAuthorizationServiceTests
{
    [Fact]
    public async Task CanManageUsersAsync_ReturnsTrue_ForSchoolAdmin()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var service = scope.ServiceProvider.GetRequiredService<IUserManagementAuthorizationService>();

        await EnsureRoleAsync(roleManager, ApplicationRoles.SchoolAdmin);

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "schooladmin@test.local",
            Email = "schooladmin@test.local",
            FullName = "School Admin",
            IsActive = true,
            SchoolId = Guid.NewGuid()
        };

        Assert.True((await userManager.CreateAsync(user)).Succeeded);
        Assert.True((await userManager.AddToRoleAsync(user, ApplicationRoles.SchoolAdmin)).Succeeded);

        var canManage = await service.CanManageUsersAsync(user.Id);
        Assert.True(canManage);
    }

    [Fact]
    public async Task CanManageUsersAsync_ReturnsFalse_ForTeacher()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var service = scope.ServiceProvider.GetRequiredService<IUserManagementAuthorizationService>();

        await EnsureRoleAsync(roleManager, ApplicationRoles.Teacher);

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "teacher@test.local",
            Email = "teacher@test.local",
            FullName = "Teacher",
            IsActive = true,
            SchoolId = Guid.NewGuid()
        };

        Assert.True((await userManager.CreateAsync(user)).Succeeded);
        Assert.True((await userManager.AddToRoleAsync(user, ApplicationRoles.Teacher)).Succeeded);

        var canManage = await service.CanManageUsersAsync(user.Id);
        Assert.False(canManage);
    }

    [Fact]
    public async Task CanManageUsersAsync_ReturnsFalse_ForMissingUser()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();

        var service = scope.ServiceProvider.GetRequiredService<IUserManagementAuthorizationService>();

        Assert.False(await service.CanManageUsersAsync(null));
        Assert.False(await service.CanManageUsersAsync(string.Empty));
        Assert.False(await service.CanManageUsersAsync("missing-user"));
    }

    private static ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase($"user-auth-{Guid.NewGuid()}"));

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IUserManagementAuthorizationService, UserManagementAuthorizationService>();

        return services.BuildServiceProvider();
    }

    private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            Assert.True((await roleManager.CreateAsync(new IdentityRole(roleName))).Succeeded);
        }
    }
}
