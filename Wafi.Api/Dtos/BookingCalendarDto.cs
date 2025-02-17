using Wafi.SampleTest.Entities;

namespace Wafi.SampleTest.Dtos
{
    public record BookingCalendarDto
    {
        public DateOnly BookingDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public RepeatOption RepeatOption { get; set; } = 0;

        public DaysOfWeek? DaysToRepeatOn { get; set; } = 0;
        public Car Car { get; set; }

        public static BookingCalendarDto ToDto(Booking booking)
        {
            return new BookingCalendarDto
            {
                BookingDate = booking.BookingDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                RepeatOption = booking.RepeatOption,
                DaysToRepeatOn = booking.DaysToRepeatOn,
                Car = booking.Car
            };
        }

        public static List<BookingCalendarDto> ToDots(List<Booking> bookings)
        {
            var dots = new List<BookingCalendarDto>(bookings.Count);
            foreach (var t in bookings) dots.Add(ToDto(t));
            return dots;
        }
    }
}