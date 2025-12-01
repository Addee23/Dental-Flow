using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DentalFlow.Pages.Bookings
{
    [Authorize(Roles = "Customer")]
    public class ReviewModel : PageModel
    {
        private readonly AppDbContext _context;
        public ReviewModel(AppDbContext context) => _context = context;

        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }

        // ✔️ Viktigt: SupportsGet behövs i din app!
        [BindProperty(SupportsGet = true)]
        public string DateTimeString { get; set; } = string.Empty;

        public DateTime SelectedDateTime { get; set; }
        public Service Service { get; set; }

        // ✔️ Viktigt: dessa ska INTE ha SupportsGet
        [BindProperty] public string Name { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Phone { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet()
        {
            if (ServiceId == 0 || string.IsNullOrWhiteSpace(DateTimeString))
                return RedirectToPage("/Services/Index");

            if (!DateTime.TryParse(DateTimeString, out DateTime parsedDate))
                return RedirectToPage("/Services/Index");

            SelectedDateTime = parsedDate;

            Service = await _context.Services.FirstOrDefaultAsync(s => s.Id == ServiceId);
            if (Service == null)
                return RedirectToPage("/Services/Index");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();

            // Parse again during POST
            if (!DateTime.TryParse(DateTimeString, out DateTime parsedDate))
                return RedirectToPage("/Services/Index");

            SelectedDateTime = parsedDate;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var booking = new Booking
            {
                UserId = userId ?? "",
                ServiceId = ServiceId,
                DateTime = SelectedDateTime,
                Name = Name,
                Email = Email,
                Phone = Phone,
                CreatedAt = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Bookings/ConfirmationSimple", new { id = booking.Id });
        }
    }
}
