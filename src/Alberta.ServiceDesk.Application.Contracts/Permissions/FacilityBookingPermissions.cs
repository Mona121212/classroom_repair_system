namespace Alberta.ServiceDesk.Permissions;

public static class FacilityBookingPermissions
{
    public const string GroupName = "FacilityBooking";

    public const string Facility = GroupName + ".Facility";
    public const string FacilityCreate = Facility + ".Create";
    public const string FacilityEdit = Facility + ".Edit";
    public const string FacilityDelete = Facility + ".Delete";
    public const string FacilityView = Facility + ".View";

    public const string Booking = GroupName + ".Booking";
    public const string BookingCreate = Booking + ".Create";
    public const string BookingEdit = Booking + ".Edit";
    public const string BookingDelete = Booking + ".Delete";
    public const string BookingView = Booking + ".View";
    public const string BookingApprove = Booking + ".Approve";
}
