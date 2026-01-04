using System;
using System.ComponentModel.DataAnnotations;

namespace Alberta.ServiceDesk.Bookings;

public class UpdateBookingDto
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    [StringLength(256)]
    public string Purpose { get; set; } = string.Empty;
}

