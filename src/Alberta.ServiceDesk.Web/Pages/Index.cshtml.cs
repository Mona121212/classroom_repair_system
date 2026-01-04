using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alberta.ServiceDesk.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Alberta.ServiceDesk.Web.Pages;

public class IndexModel : ServiceDeskPageModel
{
    public List<string> UserRoles { get; set; } = new();
    public bool IsStudent { get; set; }
    public bool IsTeacher { get; set; }
    public bool IsAdmin { get; set; }

    private readonly IdentityUserManager _userManager;

    public IndexModel(IdentityUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        if (CurrentUser.IsAuthenticated && CurrentUser.Id.HasValue)
        {
            var user = await _userManager.FindByIdAsync(CurrentUser.Id.Value.ToString());
            if (user != null)
            {
                UserRoles = (await _userManager.GetRolesAsync(user)).ToList();
                IsStudent = UserRoles.Contains(AppRoles.Student);
                IsTeacher = UserRoles.Contains(AppRoles.Teacher);
                IsAdmin = UserRoles.Contains(AppRoles.Admin);
            }
        }
    }
}
