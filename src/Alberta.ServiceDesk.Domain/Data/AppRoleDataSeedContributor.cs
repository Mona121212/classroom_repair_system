using System;
using System.Threading.Tasks;
using Alberta.ServiceDesk.Authorization;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Alberta.ServiceDesk.Data;

public class AppRoleDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;
    private readonly IGuidGenerator _guidGenerator;
    private readonly ILogger<AppRoleDataSeedContributor> _logger;

    public AppRoleDataSeedContributor(
        IdentityRoleManager roleManager,
        IGuidGenerator guidGenerator,
        ILogger<AppRoleDataSeedContributor> logger)
    {
        _roleManager = roleManager;
        _guidGenerator = guidGenerator;
        _logger = logger;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateRolesAsync();
    }

    private async Task CreateRolesAsync()
    {
        var roles = new[]
        {
            (AppRoles.Student, "Student"),
            (AppRoles.Teacher, "Teacher"),
            (AppRoles.Admin, "Admin")
        };

        foreach (var (roleName, displayName) in roles)
        {
            try
            {
                var existingRole = await _roleManager.FindByNameAsync(roleName);
                if (existingRole == null)
                {
                    var role = new IdentityRole(_guidGenerator.Create(), roleName, null)
                    {
                        IsStatic = true,
                        IsPublic = true
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"Successfully created role: {roleName}");
                    }
                    else
                    {
                        _logger.LogError($"Failed to create role {roleName}: {string.Join(", ", result.Errors)}");
                    }
                }
                else
                {
                    _logger.LogInformation($"Role {roleName} already exists, skipping creation.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating role {roleName}");
                throw;
            }
        }
    }
}
