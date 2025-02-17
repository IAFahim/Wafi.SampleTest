using Microsoft.EntityFrameworkCore;
using Wafi.Api;
using Wafi.SampleTest.Dtos;
using Wafi.SampleTest.Entities;

namespace Wafi.SampleTest.Services;

public class BookingService(WafiDbContext context) : IBookingService
{
    public async Task<List<BookingCalendarDto>> GetCalendarBookings(Guid? bookingId, Guid? carId, DateOnly? start,
        DateOnly? end)
    {
        var query = context.Bookings.Include(b => b.Car).AsQueryable();
        query = query.Where((booking) => booking.Id == bookingId);
        var bookings = await query.ToListAsync();
        return BookingCalendarDto.ToDots(bookings);
    }

    public async Task<CreateUpdateBookingDto> CreateBooking(CreateUpdateBookingDto bookingDto)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BookingCalendarDto>> GetSeedData()
    {
        var cars = await context.Cars.ToListAsync();

        if (!cars.Any())
        {
            cars = GetCars().ToList();
            await context.Cars.AddRangeAsync(cars);
            await context.SaveChangesAsync();
        }

        var bookings = await context.Bookings.ToListAsync();

        if (!bookings.Any())
        {
            bookings = GetBookings().ToList();

            await context.Bookings.AddRangeAsync(bookings);
            await context.SaveChangesAsync();
        }

        var calendar = new Dictionary<DateOnly, List<Booking>>();

        foreach (var booking in bookings)
        {
            var currentDate = booking.BookingDate;
            while (currentDate <= (booking.EndRepeatDate ?? booking.BookingDate))
            {
                if (!calendar.ContainsKey(currentDate))
                    calendar[currentDate] = new List<Booking>();

                calendar[currentDate].Add(booking);

                currentDate = booking.RepeatOption switch
                {
                    RepeatOption.Daily => currentDate.AddDays(1),
                    RepeatOption.Weekly => currentDate.AddDays(7),
                    _ => booking.EndRepeatDate.HasValue
                        ? booking.EndRepeatDate.Value.AddDays(1)
                        : currentDate.AddDays(1)
                };
            }
        }

        List<BookingCalendarDto> result = new List<BookingCalendarDto>();

        foreach (var item in calendar)
        {
            foreach (var booking in item.Value)
            {
                result.Add(new BookingCalendarDto
                {
                    BookingDate = booking.BookingDate, Car = booking.Car, StartTime = booking.StartTime,
                    EndTime = booking.EndTime
                });
            }
        }

        return result;
    }

    #region Sample Data

    private IList<Car> GetCars()
    {
        var cars = new List<Car>
        {
            new Car { Id = Guid.NewGuid(), Make = "Toyota", Model = "Corolla" },
            new Car { Id = Guid.NewGuid(), Make = "Honda", Model = "Civic" },
            new Car { Id = Guid.NewGuid(), Make = "Ford", Model = "Focus" }
        };

        return cars;
    }

    private IList<Booking> GetBookings()
    {
        var cars = GetCars();

        var bookings = new List<Booking>
        {
            new Booking
            {
                Id = Guid.NewGuid(), BookingDate = new DateOnly(2025, 2, 5), StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(12, 0, 0), RepeatOption = RepeatOption.DoesNotRepeat,
                RequestedOn = DateTime.Now, CarId = cars[0].Id, Car = cars[0]
            },
            new Booking
            {
                Id = Guid.NewGuid(), BookingDate = new DateOnly(2025, 2, 10), StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(16, 0, 0), RepeatOption = RepeatOption.Daily,
                EndRepeatDate = new DateOnly(2025, 2, 20), RequestedOn = DateTime.Now, CarId = cars[1].Id,
                Car = cars[1]
            },
            new Booking
            {
                Id = Guid.NewGuid(), BookingDate = new DateOnly(2025, 2, 15), StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 30, 0), RepeatOption = RepeatOption.Weekly,
                EndRepeatDate = new DateOnly(2025, 3, 31), RequestedOn = DateTime.Now,
                DaysToRepeatOn = DaysOfWeek.Monday, CarId = cars[2].Id, Car = cars[2]
            },
            new Booking
            {
                Id = Guid.NewGuid(), BookingDate = new DateOnly(2025, 3, 1), StartTime = new TimeSpan(11, 0, 0),
                EndTime = new TimeSpan(13, 0, 0), RepeatOption = RepeatOption.DoesNotRepeat,
                RequestedOn = DateTime.Now, CarId = cars[0].Id, Car = cars[0]
            },
            new Booking
            {
                Id = Guid.NewGuid(), BookingDate = new DateOnly(2025, 3, 7), StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(10, 0, 0), RepeatOption = RepeatOption.Weekly,
                EndRepeatDate = new DateOnly(2025, 3, 28), RequestedOn = DateTime.Now,
                DaysToRepeatOn = DaysOfWeek.Friday, CarId = cars[1].Id, Car = cars[1]
            },
            new Booking
            {
                Id = Guid.NewGuid(), BookingDate = new DateOnly(2025, 3, 15), StartTime = new TimeSpan(15, 0, 0),
                EndTime = new TimeSpan(17, 0, 0), RepeatOption = RepeatOption.Daily,
                EndRepeatDate = new DateOnly(2025, 3, 20), RequestedOn = DateTime.Now, CarId = cars[2].Id,
                Car = cars[2]
            }
        };

        return bookings;
    }

    #endregion
}