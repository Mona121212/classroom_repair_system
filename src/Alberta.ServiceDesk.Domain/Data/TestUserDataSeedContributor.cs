using System.Threading.Tasks;
using Alberta.ServiceDesk.Authorization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Alberta.ServiceDesk.Data;

public class TestUserDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityUserManager _userManager;
    private readonly IGuidGenerator _guidGenerator;

    public TestUserDataSeedContributor(
        IdentityUserManager userManager,
        IGuidGenerator guidGenerator)
    {
        _userManager = userManager;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateTestUsersAsync();
    }

    private async Task CreateTestUsersAsync()
    {
        var users = new[]
        {
            ("S1001", "Student123!", AppRoles.Student, "student@servicedesk.com", "Student User"),
            ("T1001", "Teacher123!", AppRoles.Teacher, "teacher@servicedesk.com", "Teacher User"),
            ("A1001", "Admin123!", AppRoles.Admin, "admin@servicedesk.com", "Admin User")
        };

        foreach (var (username, password, roleName, email, name) in users)
        {
            if (await _userManager.FindByNameAsync(username) == null)
            {
                var user = new IdentityUser(_guidGenerator.Create(), username, email, null)
                {
                    Name = name
                };
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
