using Microsoft.EntityFrameworkCore;
using Wafi.Api.Entities;

namespace Wafi.Api
{
    public class WafiDbContext(DbContextOptions<WafiDbContext> options) : DbContext(options)
    {
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(b => b.BookingDate)
                .HasConversion(v => v.ToString(), v => DateOnly.Parse(v));

            modelBuilder.Entity<Booking>()
                .Property(b => b.EndRepeatDate)
                .HasConversion(v => v.ToString(), v => v == null ? null : DateOnly.Parse(v));
        }
    }
}