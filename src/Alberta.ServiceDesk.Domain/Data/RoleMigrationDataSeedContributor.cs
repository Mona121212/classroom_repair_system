using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alberta.ServiceDesk.Authorization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Alberta.ServiceDesk.Data;

/// <summary>
/// Migrates users from deprecated DepartmentAdmin and SchoolAdmin roles to the new Admin role.
/// This contributor should run once to migrate existing data.
/// 
/// Note: This migration uses a simple approach - it gets all users and checks their roles.
/// For large user bases, consider using a SQL script instead.
/// </summary>
public class RoleMigrationDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;
    private readonly IdentityUserManager _userManager;
    private readonly IIdentityUserRepository _userRepository;

    public RoleMigrationDataSeedContributor(
        IdentityRoleManager roleManager,
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await MigrateOldRolesToAdminAsync();
    }

    private async Task MigrateOldRolesToAdminAsync()
    {
        // Ensure Admin role exists
        var adminRole = await _roleManager.FindByNameAsync(AppRoles.Admin);
        if (adminRole == null)
        {
            // Admin role should be created by AppRoleDataSeedContributor, but skip migration if missing
            return;
        }

        // Get all users and check their roles
        var allUsers = await _userRepository.GetListAsync();
        
        foreach (var user in allUsers)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var needsMigration = false;

            // Check if user is in DepartmentAdmin or SchoolAdmin role
            if (userRoles.Contains(AppRoles.DepartmentAdmin))
            {
                await _userManager.RemoveFromRoleAsync(user, AppRoles.DepartmentAdmin);
                needsMigration = true;
            }

            if (userRoles.Contains(AppRoles.SchoolAdmin))
            {
                await _userManager.RemoveFromRoleAsync(user, AppRoles.SchoolAdmin);
                needsMigration = true;
            }

            // Add to Admin role if migrated and not already in it
            if (needsMigration && !userRoles.Contains(AppRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, AppRoles.Admin);
            }
        }

        // Note: Permissions are already handled by AppPermissionDataSeedContributor for Admin role
        // Old role permissions will remain in AbpPermissionGrants but won't affect users
        // as they are now in Admin role which has all permissions
    }
}

