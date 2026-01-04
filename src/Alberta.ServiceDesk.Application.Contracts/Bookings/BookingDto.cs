using System;

namespace Alberta.ServiceDesk.Bookings;

public class BookingDto
{
    public Guid Id { get; set; }
    public string BookingNo { get; set; } = string.Empty;
    public Guid FacilityId { get; set; }
    public string FacilityName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public int NumberOfParticipants { get; set; }
    public int Status { get; set; } // BookingStatus enum value
    public DateTime CreationTime { get; set; }
}

