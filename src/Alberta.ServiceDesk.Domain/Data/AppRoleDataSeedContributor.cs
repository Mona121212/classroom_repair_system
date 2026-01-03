using System.Threading.Tasks;
using Alberta.ServiceDesk.Authorization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Alberta.ServiceDesk.Data;

public class AppRoleDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;
    private readonly IGuidGenerator _guidGenerator;

    public AppRoleDataSeedContributor(
        IdentityRoleManager roleManager,
        IGuidGenerator guidGenerator)
    {
        _roleManager = roleManager;
        _guidGenerator = guidGenerator;
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
            (AppRoles.DepartmentAdmin, "Department Admin"),
            (AppRoles.SchoolAdmin, "School Admin")
        };

        foreach (var (roleName, displayName) in roles)
        {
            if (await _roleManager.FindByNameAsync(roleName) == null)
            {
                var role = new IdentityRole(_guidGenerator.Create(), roleName, null)
                {
                    IsStatic = true,
                    IsPublic = true
                };
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
