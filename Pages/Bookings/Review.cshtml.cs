using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Pages.Bookings
{
    public class ReviewModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReviewModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public long UnixTicks { get; set; }

        public DateTime SelectedDateTime { get; set; }
        public Service Service { get; set; }

        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Phone { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (ServiceId == 0 || UnixTicks == 0)
                return RedirectToPage("/Services/Index");

            SelectedDateTime = new DateTime(UnixTicks);

            Service = await _context.Services
                .FirstOrDefaultAsync(s => s.Id == ServiceId);

            if (Service == null)
                return RedirectToPage("/Services/Index");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ServiceId == 0 || UnixTicks == 0)
                return RedirectToPage("/Services/Index");

            SelectedDateTime = new DateTime(UnixTicks);

            Service = await _context.Services.FirstOrDefaultAsync(s => s.Id == ServiceId);

            var booking = new Booking
            {
                ServiceId = ServiceId,
                DateTime = SelectedDateTime,
                Name = Name,
                Email = Email,
                Phone = Phone,
                CreatedAt = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Bookings/Confirmation", new { bookingId = booking.Id });
        }
    }
}
