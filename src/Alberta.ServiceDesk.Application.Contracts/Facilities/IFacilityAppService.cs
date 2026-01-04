using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Alberta.ServiceDesk.Facilities;

public interface IFacilityAppService : IApplicationService
{
    Task<PagedResultDto<FacilityDto>> GetListAsync(FacilityGetListInput input);
    Task<FacilityDto> GetAsync(Guid id);
}

