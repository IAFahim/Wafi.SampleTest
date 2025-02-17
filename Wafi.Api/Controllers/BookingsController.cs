using Microsoft.AspNetCore.Mvc;
using Wafi.Api.Dtos;
using Wafi.Api.Services;

namespace Wafi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController(IBookingService bookingService, WafiDbContext context) : ControllerBase
    {
        // GET: api/Bookings
        [HttpGet("Booking")]
        [ProducesResponseType(typeof(List<BookingCalendarDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BookingCalendarDto>>> GetCalendarBookings(
            [FromQuery] Guid? carId,
            [FromQuery] DateOnly? start,
            [FromQuery] DateOnly? end
        )
        {
            var bookings = await bookingService.GetCalendarBookings(carId, start, end);
            return Ok(bookings);
        }


        // POST: api/Bookings
        [HttpPost("Booking")]
        [ProducesResponseType(typeof(CreateUpdateBookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateUpdateBookingDto>> PostBooking(CreateUpdateBookingDto bookingDto)
        {
            var booking = await bookingService.CreateBooking(bookingDto);
            return CreatedAtAction(nameof(GetCalendarBookings), new { id = booking.Id }, booking);
        }

        // GET: api/SeedData
        // For test purpose
        [HttpGet("SeedData")]
        public async Task<ActionResult<IEnumerable<BookingCalendarDto>>> GetSeedData()
        {
            var seedData = await bookingService.GetSeedData();
            return Ok(seedData);
        }
    }
}