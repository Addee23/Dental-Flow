using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Pages.Bookings
{
    public class ConfirmationSimpleModel : PageModel
    {
        private readonly AppDbContext _context;
        public ConfirmationSimpleModel(AppDbContext context) => _context = context;

        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Booking = await _context.Bookings
                .Include(b => b.Service)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (Booking == null)
                return RedirectToPage("/Services/Index");

            return Page();
        }
    }
}
