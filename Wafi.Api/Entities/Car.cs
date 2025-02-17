using System.ComponentModel.DataAnnotations;

namespace Wafi.SampleTest.Entities
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
