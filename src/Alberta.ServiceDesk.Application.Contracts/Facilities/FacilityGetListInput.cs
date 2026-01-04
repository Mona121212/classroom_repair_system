using Volo.Abp.Application.Dtos;

namespace Alberta.ServiceDesk.Facilities;

public class FacilityGetListInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// Filter by campus (maps to OwnerUnit field)
    /// </summary>
    public string? Campus { get; set; }
    
    /// <summary>
    /// Filter by facility type (e.g., "Lab", "Auditorium", "Sports")
    /// </summary>
    public string? Type { get; set; }
    
    /// <summary>
    /// Minimum capacity filter
    /// </summary>
    public int? MinCapacity { get; set; }
    
    /// <summary>
    /// Maximum capacity filter
    /// </summary>
    public int? MaxCapacity { get; set; }
}

