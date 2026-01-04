using System.Threading.Tasks;
using Alberta.ServiceDesk.Authorization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace Alberta.ServiceDesk.Data;

public class AppPermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IdentityRoleManager _roleManager;

    // Permission constants matching FacilityBookingPermissions
    private const string FacilityBookingGroup = "FacilityBooking";
    private const string Facility = FacilityBookingGroup + ".Facility";
    private const string FacilityView = Facility + ".View";
    private const string FacilityCreate = Facility + ".Create";
    private const string FacilityEdit = Facility + ".Edit";
    private const string FacilityDelete = Facility + ".Delete";
    private const string Booking = FacilityBookingGroup + ".Booking";
    private const string BookingView = Booking + ".View";
    private const string BookingCreate = Booking + ".Create";
    private const string BookingEdit = Booking + ".Edit";
    private const string BookingDelete = Booking + ".Delete";
    private const string BookingApprove = Booking + ".Approve";

    public AppPermissionDataSeedContributor(
        IPermissionDataSeeder permissionDataSeeder,
        IdentityRoleManager roleManager)
    {
        _permissionDataSeeder = permissionDataSeeder;
        _roleManager = roleManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedPermissionsAsync(context);
    }

    private async Task SeedPermissionsAsync(DataSeedContext context)
    {
        // Student permissions: Facility.View, Booking.View/Create/Edit/Delete (but cannot book Labs - business rule)
        var studentRole = await _roleManager.FindByNameAsync(AppRoles.Student);
        if (studentRole != null)
        {
            await _permissionDataSeeder.SeedAsync(
                "R", // Role permission value provider
                AppRoles.Student,
                new[]
                {
                    FacilityView,
                    BookingView,
                    BookingCreate,
                    BookingEdit,
                    BookingDelete
                },
                context.TenantId);
        }

        // Teacher permissions: Facility.View, Booking.View/Create/Edit/Delete
        var teacherRole = await _roleManager.FindByNameAsync(AppRoles.Teacher);
        if (teacherRole != null)
        {
            await _permissionDataSeeder.SeedAsync(
                "R", // Role permission value provider
                AppRoles.Teacher,
                new[]
                {
                    FacilityView,
                    BookingView,
                    BookingCreate,
                    BookingEdit,
                    BookingDelete
                },
                context.TenantId);
        }

        // Admin permissions: Full access to everything
        var adminRole = await _roleManager.FindByNameAsync(AppRoles.Admin);
        if (adminRole != null)
        {
            await _permissionDataSeeder.SeedAsync(
                "R", // Role permission value provider
                AppRoles.Admin,
                new[]
                {
                    FacilityView,
                    FacilityCreate,
                    FacilityEdit,
                    FacilityDelete,
                    BookingView,
                    BookingCreate,
                    BookingEdit,
                    BookingDelete,
                    BookingApprove
                },
                context.TenantId);
        }
    }
}
