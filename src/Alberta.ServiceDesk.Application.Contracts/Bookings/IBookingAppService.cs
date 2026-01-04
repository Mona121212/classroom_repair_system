using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace Alberta.ServiceDesk.Bookings;

public interface IBookingAppService : IApplicationService
{
    Task<BookingDto> CreateAsync(CreateBookingDto input);
    Task<BookingDto> UpdateAsync(Guid id, UpdateBookingDto input);
    Task DeleteAsync(Guid id);
    Task<BookingDto> GetAsync(Guid id);
    Task<List<BookingDto>> GetListAsync();
}

