using Alberta.ServiceDesk.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Alberta.ServiceDesk.Permissions;

public class FacilityBookingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var facilityBookingGroup = context.AddGroup(
            FacilityBookingPermissions.GroupName,
            L("Permission:FacilityBooking"));

        var facilityPermission = facilityBookingGroup.AddPermission(
            FacilityBookingPermissions.Facility,
            L("Permission:Facility"));
        facilityPermission.AddChild(FacilityBookingPermissions.FacilityCreate, L("Permission:Facility.Create"));
        facilityPermission.AddChild(FacilityBookingPermissions.FacilityEdit, L("Permission:Facility.Edit"));
        facilityPermission.AddChild(FacilityBookingPermissions.FacilityDelete, L("Permission:Facility.Delete"));
        facilityPermission.AddChild(FacilityBookingPermissions.FacilityView, L("Permission:Facility.View"));

        var bookingPermission = facilityBookingGroup.AddPermission(
            FacilityBookingPermissions.Booking,
            L("Permission:Booking"));
        bookingPermission.AddChild(FacilityBookingPermissions.BookingCreate, L("Permission:Booking.Create"));
        bookingPermission.AddChild(FacilityBookingPermissions.BookingEdit, L("Permission:Booking.Edit"));
        bookingPermission.AddChild(FacilityBookingPermissions.BookingDelete, L("Permission:Booking.Delete"));
        bookingPermission.AddChild(FacilityBookingPermissions.BookingView, L("Permission:Booking.View"));
        bookingPermission.AddChild(FacilityBookingPermissions.BookingApprove, L("Permission:Booking.Approve"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ServiceDeskResource>(name);
    }
}
