using System.Text.Json;
using BlossomTreeManager.Web.Data;
using BlossomTreeManager.Web.Data.Entities;

namespace BlossomTreeManager.Web.Services;

public record TenantScope(Guid? SchoolId, bool IsGlobalUser, string? UserId);

public interface ITenantGuardService
{
    Task<TenantScope> GetScopeAsync();
    Task RequireSchoolMutationAccessAsync(
        Guid targetSchoolId,
        string action,
        string entityName,
        string entityId,
        object? deniedMetadata = null);
    Task LogMutationAsync(
        string action,
        string entityName,
        string entityId,
        Guid? schoolId,
        object? metadata = null);
}

public class TenantGuardService(
    ISchoolContext schoolContext,
    ApplicationDbContext dbContext) : ITenantGuardService
{
    public async Task<TenantScope> GetScopeAsync()
    {
        var schoolId = await schoolContext.GetSchoolIdAsync();
        var isGlobalUser = await schoolContext.IsGlobalUserAsync();
        var userId = await schoolContext.GetUserIdAsync();

        return new TenantScope(schoolId, isGlobalUser, userId);
    }

    public async Task RequireSchoolMutationAccessAsync(
        Guid targetSchoolId,
        string action,
        string entityName,
        string entityId,
        object? deniedMetadata = null)
    {
        var scope = await GetScopeAsync();
        if (scope.IsGlobalUser)
        {
            return;
        }

        var denied = !scope.SchoolId.HasValue || scope.SchoolId.Value != targetSchoolId;
        if (!denied)
        {
            return;
        }

        await TryWriteDeniedAuditAsync(action, entityName, entityId, targetSchoolId, scope.UserId, deniedMetadata);

        throw new UnauthorizedAccessException("This mutation is outside the current school scope.");
    }

    public async Task LogMutationAsync(
        string action,
        string entityName,
        string entityId,
        Guid? schoolId,
        object? metadata = null)
    {
        var userId = await schoolContext.GetUserIdAsync();

        var entry = new AuditLogEntry
        {
            SchoolId = schoolId,
            ActorUserId = userId,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            MetadataJson = SerializeMetadata(metadata)
        };

        dbContext.AuditLogs.Add(entry);
        await dbContext.SaveChangesAsync();
    }

    private async Task TryWriteDeniedAuditAsync(
        string action,
        string entityName,
        string entityId,
        Guid targetSchoolId,
        string? userId,
        object? deniedMetadata)
    {
        try
        {
            var entry = new AuditLogEntry
            {
                SchoolId = targetSchoolId,
                ActorUserId = userId,
                Action = $"DENIED:{action}",
                EntityName = entityName,
                EntityId = entityId,
                MetadataJson = SerializeMetadata(deniedMetadata)
            };

            dbContext.AuditLogs.Add(entry);
            await dbContext.SaveChangesAsync();
        }
        catch
        {
            // Preserve the authorization failure even if audit writing fails.
        }
    }

    private static string? SerializeMetadata(object? metadata)
    {
        if (metadata is null)
        {
            return null;
        }

        return JsonSerializer.Serialize(metadata);
    }
}
