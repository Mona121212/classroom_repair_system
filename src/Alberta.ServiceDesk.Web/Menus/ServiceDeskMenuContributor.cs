using System.Threading.Tasks;
using Alberta.ServiceDesk.Localization;
using Alberta.ServiceDesk.Permissions;
using Alberta.ServiceDesk.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;

namespace Alberta.ServiceDesk.Web.Menus;

public class ServiceDeskMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<ServiceDeskResource>();
        var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                ServiceDeskMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        //Facilities - Visible to all authenticated users
        context.Menu.AddItem(
            new ApplicationMenuItem(
                ServiceDeskMenus.Facilities,
                l["Menu:Facilities"],
                "~/Facilities",
                icon: "fa fa-building",
                order: 2,
                requiredPermissionName: FacilityBookingPermissions.FacilityView
            )
        );

        //Bookings - Visible to all authenticated users
        context.Menu.AddItem(
            new ApplicationMenuItem(
                ServiceDeskMenus.Bookings,
                l["Menu:Bookings"],
                "~/Bookings",
                icon: "fa fa-calendar-check",
                order: 3,
                requiredPermissionName: FacilityBookingPermissions.BookingView
            )
        );

        //Administration - Only visible to Admin
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;
        administration.RequiredPermissionName = FacilityBookingPermissions.FacilityCreate; // Only Admin has this permission

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);
    
        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }
        
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 8);
        
        return Task.CompletedTask;
    }
}
