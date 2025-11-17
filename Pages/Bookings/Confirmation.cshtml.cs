using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Pages.Bookings
{
    public class ConfirmationModel : PageModel
    {
        private readonly AppDbContext _context;

        public ConfirmationModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int BookingId { get; set; }

        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (BookingId == 0)
                return RedirectToPage("/Services/Index");

            Booking = await _context.Bookings
                .Include(b => b.Service)
                .FirstOrDefaultAsync(b => b.Id == BookingId);

            if (Booking == null)
                return RedirectToPage("/Services/Index");

            return Page();
        }
    }
}
