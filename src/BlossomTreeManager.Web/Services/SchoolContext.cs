using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Constants;

namespace BlossomTreeManager.Web.Services;

public interface ISchoolContext
{
    Task<Guid?> GetSchoolIdAsync();
    Task<bool> IsGlobalUserAsync();
    Task<string?> GetUserIdAsync();
}

public class SchoolContext(
    AuthenticationStateProvider authenticationStateProvider,
    UserManager<ApplicationUser> userManager) : ISchoolContext
{
    public async Task<Guid?> GetSchoolIdAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.SchoolId;
    }

    public async Task<bool> IsGlobalUserAsync()
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
        {
            return false;
        }

        return await userManager.IsInRoleAsync(user, ApplicationRoles.AppOwner)
               || await userManager.IsInRoleAsync(user, ApplicationRoles.CompanyStaff);
    }

    public async Task<string?> GetUserIdAsync()
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();
        return state.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    private async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();
        var principal = state.User;
        if (principal.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return await userManager.GetUserAsync(principal);
    }
}
