using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wafi.Api;
using Wafi.Api.Dtos;
using Wafi.Api.Services;
using Xunit.Abstractions;

namespace Test.Wafi.Api
{
    public class BookingServiceTests : IAsyncLifetime
    {
        private readonly WafiDbContext _context;
        private readonly ITestOutputHelper _testOutputHelper;

        public BookingServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var options = new DbContextOptionsBuilder<WafiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCarBooking")
                .Options;

            _context = new WafiDbContext(options);
        }

        [Fact]
        public async Task GetCalendarBookings_ReturnsEmptyList_WhenNoDataExistsForFilter()
        {
            // Arrange
            var service = new BookingService(_context);
            var seeded = await service.GetSeedData();
            await _context.SaveChangesAsync();
            BookingCalendarDto calendarDto = seeded.First();

            // Act
            var result = await service.GetCalendarBookings(calendarDto.Car.Id, null, null);

            // Assert
            Assert.NotNull(result);
            foreach (var bookingCalendarDto in result)
            {
                _testOutputHelper.WriteLine(bookingCalendarDto.ToString());
            }
        }

        [Fact]
        public async Task CreateBooking_ThrowsNotImplementedException()
        {
            // Arrange
            var service = new BookingService(_context);
            var bookingDto = new CreateUpdateBookingDto(); // Dummy DTO

            // Act & Assert
            await Assert.ThrowsAsync<NotImplementedException>(() => service.CreateBooking(bookingDto));
        }


        [Fact]
        public async Task GetSeedData_ReturnsSeedData()
        {
            // Arrange
            var service = new BookingService(_context);

            // Act
            var result = await service.GetSeedData();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result); // Assuming GetSeedData always returns some data
            result.Should().AllBeOfType<BookingCalendarDto>();

            // Verify data was actually seeded into the in-memory database (optional, but good practice)
            var carsInDb = await _context.Cars.ToListAsync();
            var bookingsInDb = await _context.Bookings.ToListAsync();

            Assert.NotEmpty(carsInDb);
            Assert.NotEmpty(bookingsInDb);
        }

        public async Task InitializeAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}