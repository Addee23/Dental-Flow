using DentalFlow.Models;
using DentalFlow.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ⭐ GÖR UserId FRIVILLIG (NULLABLE) ⭐
            modelBuilder.Entity<Booking>()
                .Property(b => b.UserId)
                .IsRequired(false);
        }
    }
}
