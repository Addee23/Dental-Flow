using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DentalFlow.Pages.Bookings
{
    [Authorize(Roles = "Customer")]
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
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public List<DateTime> AvailableTimes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (ServiceId == 0)
                return RedirectToPage("/Services/Index");

            await LoadTimes();
            return Page();
        }

        private async Task LoadTimes()
        {
            var booked = await _context.Bookings
                .Where(b => b.ServiceId == ServiceId &&
                            b.DateTime.Date == SelectedDate.Date)
                .Select(b => b.DateTime)
                .ToListAsync();

            AvailableTimes = Enumerable.Range(8, 9)
                .Select(h => new DateTime(
                    SelectedDate.Year,
                    SelectedDate.Month,
                    SelectedDate.Day,
                    h, 0, 0))
                .Where(t => !booked.Contains(t))
                .ToList();
        }
    }
}
