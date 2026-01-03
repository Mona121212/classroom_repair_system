using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Alberta.ServiceDesk.Facilities;

public class Facility : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OwnerUnit { get; set; }
    public string? Type { get; set; }
    public bool RequiresApproval { get; set; }
    public int? Capacity { get; set; }

    protected Facility()
    {
    }

    public Facility(Guid id) : base(id)
    {
    }
}
