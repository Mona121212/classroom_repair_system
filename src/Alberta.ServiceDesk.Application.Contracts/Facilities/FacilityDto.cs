using System;

namespace Alberta.ServiceDesk.Facilities;

public class FacilityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OwnerUnit { get; set; }
    public string? Type { get; set; }
    public bool RequiresApproval { get; set; }
    public int? Capacity { get; set; }
    public DateTime CreationTime { get; set; }
}

