using DentalFlow.Data;        
using DentalFlow.Models;

namespace DentalFlow.Data.Seed
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Services.Any()) return;

            context.Services.AddRange(
                new Service { Name = "Tandundersökning", DurationMinutes = 45, Price = 850 },
                new Service { Name = "Tandblekning", DurationMinutes = 60, Price = 950 },
                new Service { Name = "Akutbesök", DurationMinutes = 30, Price = 650 }
            );

            context.SaveChanges();
        }
    }
}
