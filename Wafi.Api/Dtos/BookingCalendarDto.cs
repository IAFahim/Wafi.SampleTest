using Wafi.Api.Entities;

namespace Wafi.Api.Dtos
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


        public override string ToString()
        {
            string daysOfWeekString = DaysToRepeatOn.HasValue ? DaysToRepeatOn.Value.ToString() : "None";
            string carString = $"Car: {Car.Make} {Car.Model} (Id: {Car.Id})";

            return $$"""
                     BookingCalendarDto {
                         BookingDate: {{BookingDate:yyyy-MM-dd}},
                         StartTime: {{StartTime:c}},
                         EndTime: {{EndTime:c}}, 
                         RepeatOption: {{RepeatOption}},
                         DaysToRepeatOn: {{daysOfWeekString}},
                         {{carString}}
                     }
                     """;
        }
    }
}