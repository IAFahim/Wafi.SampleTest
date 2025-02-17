using Wafi.Api.Dtos;

namespace Wafi.Api.Services;

public interface IBookingService
{
    Task<List<BookingCalendarDto>> GetCalendarBookings(Guid? carId, DateOnly? start, DateOnly? end);
    Task<CreateUpdateBookingDto> CreateBooking(CreateUpdateBookingDto bookingDto);
    Task<IEnumerable<BookingCalendarDto>> GetSeedData();
}