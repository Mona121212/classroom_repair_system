using System;
using Alberta.ServiceDesk.Facilities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Alberta.ServiceDesk.Bookings;

public class Booking : FullAuditedAggregateRoot<Guid>
{
    public string BookingNo { get; set; } = string.Empty;
    public Guid FacilityId { get; set; }
    public Facility? Facility { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public int NumberOfParticipants { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Draft;

    protected Booking()
    {
    }

    public Booking(Guid id) : base(id)
    {
    }
}
