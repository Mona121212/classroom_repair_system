using Alberta.ServiceDesk.Bookings;
using Alberta.ServiceDesk.Facilities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Alberta.ServiceDesk;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ServiceDeskApplicationMappers
{
    public partial BookingDto Map(Booking source);
    public partial void Map(Booking source, BookingDto destination);
    
    public partial FacilityDto Map(Facility source);
    public partial void Map(Facility source, FacilityDto destination);
}
