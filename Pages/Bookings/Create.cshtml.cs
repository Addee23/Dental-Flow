using DentalFlow.Data;
using DentalFlow.Models;
using DentalFlow.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DentalFlow.Pages.Bookings
{
    [Authorize(Roles = "Customer")]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public Service Service { get; set; }

        public IActionResult OnGet(int serviceId)
        {
            Service = _context.Services.FirstOrDefault(s => s.Id == serviceId);

            if (Service == null)
                return NotFound();

            Booking = new Booking
            {
                ServiceId = serviceId,
                DateTime = DateTime.Now.AddDays(1)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // ⭐⭐⭐ HÄR ska raden läggas in ⭐⭐⭐
            Booking.UserId = _userManager.GetUserId(User);

            // ⭐⭐⭐ Denna OCKSÅ ⭐⭐⭐
            Booking.CreatedAt = DateTime.Now;

            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("Confirmation", new { id = Booking.Id });
        }
    }
}
