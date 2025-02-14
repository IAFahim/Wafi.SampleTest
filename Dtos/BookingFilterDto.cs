using System.ComponentModel.DataAnnotations;

namespace Wafi.SampleTest.Dtos
{
    public class BookingFilterDto
    {
        public Guid? CarId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly? StartBookingDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly? EndBookingDate { get; set; }
    }
}
