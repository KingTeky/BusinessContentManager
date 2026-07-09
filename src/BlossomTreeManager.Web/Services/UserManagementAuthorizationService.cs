using Microsoft.AspNetCore.Identity;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Constants;

namespace BlossomTreeManager.Web.Services;

public interface IUserManagementAuthorizationService
{
    Task<bool> CanManageUsersAsync(string? userId);
}

public class UserManagementAuthorizationService(UserManager<ApplicationUser> userManager) : IUserManagementAuthorizationService
{
    public async Task<bool> CanManageUsersAsync(string? userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        var currentUser = await userManager.FindByIdAsync(userId);
        if (currentUser is null)
        {
            return false;
        }

        return await userManager.IsInRoleAsync(currentUser, ApplicationRoles.AppOwner)
               || await userManager.IsInRoleAsync(currentUser, ApplicationRoles.SchoolAdmin);
    }
}
