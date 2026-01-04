using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Alberta.ServiceDesk.Facilities;
using Alberta.ServiceDesk.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Alberta.ServiceDesk.Facilities;

[Authorize(FacilityBookingPermissions.FacilityView)]
public class FacilityAppService : ApplicationService, IFacilityAppService
{
    private readonly IRepository<Facility, Guid> _facilityRepository;

    public FacilityAppService(IRepository<Facility, Guid> facilityRepository)
    {
        _facilityRepository = facilityRepository;
    }

    public async Task<PagedResultDto<FacilityDto>> GetListAsync(FacilityGetListInput input)
    {
        var queryable = await _facilityRepository.GetQueryableAsync();

        // Apply filters
        // Campus filter maps to OwnerUnit field
        if (!string.IsNullOrWhiteSpace(input.Campus))
        {
            queryable = queryable.Where(x => x.OwnerUnit != null && x.OwnerUnit.Contains(input.Campus));
        }

        if (!string.IsNullOrWhiteSpace(input.Type))
        {
            queryable = queryable.Where(x => x.Type != null && x.Type.Contains(input.Type));
        }

        if (input.MinCapacity.HasValue)
        {
            queryable = queryable.Where(x => x.Capacity.HasValue && x.Capacity >= input.MinCapacity.Value);
        }

        if (input.MaxCapacity.HasValue)
        {
            queryable = queryable.Where(x => x.Capacity.HasValue && x.Capacity <= input.MaxCapacity.Value);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(input.Sorting))
        {
            queryable = queryable.OrderBy(input.Sorting);
        }
        else
        {
            queryable = queryable.OrderBy(x => x.Name);
        }

        // Get total count
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        // Apply paging
        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        // Execute query
        var facilities = await AsyncExecuter.ToListAsync(queryable);

        // Map to DTOs
        var facilityDtos = ObjectMapper.Map<System.Collections.Generic.List<Facility>, System.Collections.Generic.List<FacilityDto>>(facilities);

        return new PagedResultDto<FacilityDto>(totalCount, facilityDtos);
    }

    public async Task<FacilityDto> GetAsync(Guid id)
    {
        var facility = await _facilityRepository.GetAsync(id);
        return ObjectMapper.Map<Facility, FacilityDto>(facility);
    }
}

