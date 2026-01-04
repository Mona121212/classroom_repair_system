using System;
using System.ComponentModel.DataAnnotations;

namespace Alberta.ServiceDesk.Bookings;

public class CreateBookingDto
{
    [Required]
    public Guid FacilityId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [StringLength(256)]
    public string Purpose { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Number of participants must be at least 1")]
    public int NumberOfParticipants { get; set; } = 1;
}

