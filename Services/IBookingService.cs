using Wafi.SampleTest.Dtos;

namespace Wafi.SampleTest.Services;

public interface IBookingService
{
    Task<List<BookingCalendarDto>> GetCalendarBookings(Guid? bookingId, Guid? carId, DateOnly? start, DateOnly? end);
    Task<CreateUpdateBookingDto> CreateBooking(CreateUpdateBookingDto bookingDto);
    Task<IEnumerable<BookingCalendarDto>> GetSeedData();
}