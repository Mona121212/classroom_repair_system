using System;
using System.Collections.Generic;
using System.Text;

namespace Alberta.ServiceDesk.Bookings
{
    public enum BookingStatus
    {
        Draft = 0,
        Submitted = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4,
        Completed = 5,
        Overridden = 6
    }
}
