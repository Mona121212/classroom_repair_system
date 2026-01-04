using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alberta.ServiceDesk.Authorization;
using Alberta.ServiceDesk.Facilities;
using Alberta.ServiceDesk.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Alberta.ServiceDesk.Bookings;

[Authorize(FacilityBookingPermissions.Booking)]
public class BookingAppService : ApplicationService, IBookingAppService
{
    private readonly IRepository<Booking, Guid> _bookingRepository;
    private readonly IRepository<Facility, Guid> _facilityRepository;
    private readonly IdentityUserManager _userManager;

    public BookingAppService(
        IRepository<Booking, Guid> bookingRepository,
        IRepository<Facility, Guid> facilityRepository,
        IdentityUserManager userManager)
    {
        _bookingRepository = bookingRepository;
        _facilityRepository = facilityRepository;
        _userManager = userManager;
    }

    [Authorize(FacilityBookingPermissions.BookingCreate)]
    public async Task<BookingDto> CreateAsync(CreateBookingDto input)
    {
        // Business Rule 1: Validate time range
        if (input.EndTime <= input.StartTime)
        {
            throw new UserFriendlyException(
                "End time must be after start time.",
                "The booking end time must be later than the start time."
            );
        }

        // Business Rule 2: Students cannot book Labs
        var facility = await _facilityRepository.GetAsync(input.FacilityId);
        await CheckLabBookingRestrictionAsync(input.FacilityId);

        // Business Rule 3: Validate capacity
        if (facility.Capacity.HasValue && input.NumberOfParticipants > facility.Capacity.Value)
        {
            throw new UserFriendlyException(
                $"Number of participants ({input.NumberOfParticipants}) exceeds facility capacity ({facility.Capacity.Value}).",
                "The number of participants cannot exceed the facility's capacity."
            );
        }

        // Generate BookingNo
        var bookingNo = await GenerateBookingNoAsync();
        
        var booking = new Booking(GuidGenerator.Create())
        {
            BookingNo = bookingNo,
            FacilityId = input.FacilityId,
            StartTime = input.StartTime,
            EndTime = input.EndTime,
            Purpose = input.Purpose,
            NumberOfParticipants = input.NumberOfParticipants,
            Status = BookingStatus.Draft
        };

        await _bookingRepository.InsertAsync(booking);

        var bookingDto = ObjectMapper.Map<Booking, BookingDto>(booking);
        bookingDto.FacilityName = facility.Name;
        return bookingDto;
    }

    /// <summary>
    /// Generates a unique booking number in format: BK-YYYYMMDD-XXXX
    /// </summary>
    private async Task<string> GenerateBookingNoAsync()
    {
        var datePrefix = DateTime.Now.ToString("yyyyMMdd");
        var baseNo = $"BK-{datePrefix}-";
        
        // Find the highest sequence number for today
        var todayStart = DateTime.Today;
        var todayEnd = todayStart.AddDays(1);
        
        var queryable = await _bookingRepository.GetQueryableAsync();
        var allBookings = await AsyncExecuter.ToListAsync(queryable);
        var todayBookings = allBookings
            .Where(b => b.BookingNo.StartsWith(baseNo) && b.CreationTime >= todayStart && b.CreationTime < todayEnd)
            .ToList();
        
        int sequence = 1;
        if (todayBookings.Any())
        {
            var maxSequence = todayBookings
                .Select(b => 
                {
                    var parts = b.BookingNo.Split('-');
                    return parts.Length >= 3 && int.TryParse(parts[2], out var seq) ? seq : 0;
                })
                .DefaultIfEmpty(0)
                .Max();
            sequence = maxSequence + 1;
        }
        
        return $"{baseNo}{sequence:D4}";
    }

    [Authorize(FacilityBookingPermissions.BookingEdit)]
    public async Task<BookingDto> UpdateAsync(Guid id, UpdateBookingDto input)
    {
        var booking = await _bookingRepository.GetAsync(id, includeDetails: true);

        // Business Rule: Students cannot book Labs (check existing booking's facility)
        await CheckLabBookingRestrictionAsync(booking.FacilityId);

        booking.StartTime = input.StartTime;
        booking.EndTime = input.EndTime;
        booking.Purpose = input.Purpose;

        await _bookingRepository.UpdateAsync(booking);

        var bookingDto = ObjectMapper.Map<Booking, BookingDto>(booking);
        if (booking.Facility != null)
        {
            bookingDto.FacilityName = booking.Facility.Name;
        }
        return bookingDto;
    }

    [Authorize(FacilityBookingPermissions.BookingDelete)]
    public async Task DeleteAsync(Guid id)
    {
        await _bookingRepository.DeleteAsync(id);
    }

    [Authorize(FacilityBookingPermissions.BookingView)]
    public async Task<BookingDto> GetAsync(Guid id)
    {
        var booking = await _bookingRepository.GetAsync(id, includeDetails: true);
        var bookingDto = ObjectMapper.Map<Booking, BookingDto>(booking);
        if (booking.Facility != null)
        {
            bookingDto.FacilityName = booking.Facility.Name;
        }
        return bookingDto;
    }

    [Authorize(FacilityBookingPermissions.BookingView)]
    public async Task<List<BookingDto>> GetListAsync()
    {
        var queryable = await _bookingRepository.GetQueryableAsync();
        
        // Check if current user is Admin - Admin can see all bookings
        bool isAdmin = false;
        if (CurrentUser.Id.HasValue)
        {
            var currentUser = await _userManager.FindByIdAsync(CurrentUser.Id.Value.ToString());
            if (currentUser != null)
            {
                var userRoles = await _userManager.GetRolesAsync(currentUser);
                isAdmin = userRoles.Contains(AppRoles.Admin);
            }
        }
        
        // If not Admin, filter by current user's bookings only
        if (!isAdmin && CurrentUser.Id.HasValue)
        {
            queryable = queryable.Where(b => b.CreatorId == CurrentUser.Id.Value);
        }
        
        var bookings = await AsyncExecuter.ToListAsync(queryable);
        var bookingDtos = ObjectMapper.Map<List<Booking>, List<BookingDto>>(bookings);
        
        // Populate FacilityName - need to load facilities
        var facilityIds = bookings.Select(b => b.FacilityId).Distinct().ToList();
        var facilities = await _facilityRepository.GetListAsync(f => facilityIds.Contains(f.Id));
        var facilityDict = facilities.ToDictionary(f => f.Id, f => f.Name);
        
        foreach (var dto in bookingDtos)
        {
            if (facilityDict.TryGetValue(dto.FacilityId, out var facilityName))
            {
                dto.FacilityName = facilityName;
            }
        }
        
        return bookingDtos;
    }

    /// <summary>
    /// Business Rule: Students cannot book Lab facilities.
    /// This is a business rule check, not a permission check.
    /// Students have Booking.Create permission, but cannot book Labs.
    /// </summary>
    private async Task CheckLabBookingRestrictionAsync(Guid facilityId)
    {
        var facility = await _facilityRepository.GetAsync(facilityId);
        
        // Check if facility is a Lab
        if (facility.Type?.Equals("Lab", StringComparison.OrdinalIgnoreCase) == true)
        {
            // Check if current user is a Student
            if (CurrentUser.Id.HasValue)
            {
                var currentUser = await _userManager.FindByIdAsync(CurrentUser.Id.Value.ToString());
                if (currentUser != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(currentUser);
                    
                    if (userRoles.Contains(AppRoles.Student))
                    {
                        throw new UserFriendlyException(
                            "Students cannot book laboratory facilities. Please contact a teacher or administrator for assistance.",
                            "Laboratory facilities are restricted to teachers and administrators only."
                        );
                    }
                }
            }
        }
    }
}

