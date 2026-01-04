import { apiClient } from './api';
import type { FacilityDto, FacilityGetListInput } from '../types/facility';
import type { PagedResultDto } from '../types/common';

export const facilitiesApi = {
  getList: async (input: FacilityGetListInput): Promise<PagedResultDto<FacilityDto>> => {
    const params = new URLSearchParams();
    if (input.campus) params.append('Campus', input.campus);
    if (input.type) params.append('Type', input.type);
    if (input.minCapacity) params.append('MinCapacity', input.minCapacity.toString());
    if (input.maxCapacity) params.append('MaxCapacity', input.maxCapacity.toString());
    if (input.skipCount) params.append('SkipCount', input.skipCount.toString());
    if (input.maxResultCount) params.append('MaxResultCount', input.maxResultCount.toString());
    if (input.sorting) params.append('Sorting', input.sorting);

    const queryString = params.toString();
    return apiClient.get<PagedResultDto<FacilityDto>>(
      queryString ? `/api/app/facility?${queryString}` : '/api/app/facility'
    );
  },

  getById: async (id: string): Promise<FacilityDto> => {
    return apiClient.get<FacilityDto>(`/api/app/facility/${id}`);
  },
};

