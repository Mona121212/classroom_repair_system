using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace Alberta.ServiceDesk.Data
{
    public class ServiceDeskDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityRoleManager _roleManager;
        private readonly IdentityUserManager _userManager;
        private readonly IGuidGenerator _guidGenerator;

        public ServiceDeskDataSeedContributor(
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            IdentityRoleManager roleManager,
            IdentityUserManager userManager,
            IGuidGenerator guidGenerator)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await CreateRolesAsync();
            await CreateUsersAsync();
        }

        private async Task CreateRolesAsync()
        {
            var roles = new[]
            {
                ("TEACHER", "Teacher"),
                ("TECHNICIAN", "Technician"),
                ("MANAGER", "Manager")
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

        private async Task CreateUsersAsync()
        {

            var users = new[]
            {
                ("T1001", "Teacher123!", "TEACHER", "teacher@servicedesk.com"),
                ("E1001", "Tech123!", "TECHNICIAN", "technician@servicedesk.com"),
                ("M1001", "Manager123!", "MANAGER", "manager@servicedesk.com")
            };

            foreach (var (username, password, roleName, email) in users)
            {
                if (await _userManager.FindByNameAsync(username) == null)
                {
                    var user = new IdentityUser(_guidGenerator.Create(), username, email, null)
                    {
                        Name = $"{roleName} User"
                    };
                    await _userManager.CreateAsync(user, password);
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
