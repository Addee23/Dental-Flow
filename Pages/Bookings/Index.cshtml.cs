using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime SelectedDate { get; set; }

        public List<TimeSlot> AvailableTimes { get; set; } = new();

        public class TimeSlot
        {
            public DateTime Time { get; set; }
            public bool IsPast { get; set; }
            public bool IsBooked { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (SelectedDate == DateTime.MinValue)
                SelectedDate = DateTime.Today;

            if (SelectedDate < DateTime.Today)
                SelectedDate = DateTime.Today;

            var now = DateTime.Now;

            // Hämta redan bokade tider
            var bookedTimes = await _context.Bookings
                .Where(b => b.ServiceId == ServiceId &&
                            b.DateTime.Date == SelectedDate.Date)
                .Select(b => b.DateTime.TimeOfDay)
                .ToListAsync();

            // Generera tider 08–17
            for (int hour = 8; hour <= 17; hour++)
            {
                var t = new DateTime(
                    SelectedDate.Year,
                    SelectedDate.Month,
                    SelectedDate.Day,
                    hour, 0, 0
                );

                AvailableTimes.Add(new TimeSlot
                {
                    Time = t,
                    IsPast = (SelectedDate == DateTime.Today && t <= now),
                    IsBooked = bookedTimes.Contains(t.TimeOfDay)
                });
            }

            return Page();
        }
    }
}
