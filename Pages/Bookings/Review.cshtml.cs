using DentalFlow.Data;
using DentalFlow.Models;
using DentalFlow.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DentalFlow.Pages.Bookings
{
    [Authorize(Roles = "Customer")]
    public class ReviewModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;


        public ReviewModel(AppDbContext context,
                   UserManager<ApplicationUser> userManager,
                   EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }


        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DateTimeString { get; set; } = string.Empty;

        public DateTime SelectedDateTime { get; set; }
        public Service Service { get; set; }

        // Dessa fylls med användarens info
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

            // Hämta behandling
            Service = await _context.Services.FirstOrDefaultAsync(s => s.Id == ServiceId);
            if (Service == null)
                return RedirectToPage("/Services/Index");

            // Hämta inloggad användare
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                Name = $"{user.FirstName} {user.LastName}";
                Email = user.Email;
                Phone = user.PhoneNumber;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Clear();

            var user = await _userManager.GetUserAsync(User);
            Email = user?.Email ?? "";
            Name = $"{user?.FirstName} {user?.LastName}";
            Phone = user?.PhoneNumber;

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

            //  SKICKA BEKRÄFTELSEMAIL
            var service = await _context.Services.FindAsync(ServiceId);

            await _emailService.SendEmailAsync(
                Email,
                "Bokningsbekräftelse – DentalFlow",
                $"Hej {Name}!\n\n" +
                $"Din bokning är bekräftad.\n" +
                $"Behandling: {service?.Name}\n" +
                $"Datum: {SelectedDateTime:yyyy-MM-dd}\n" +
                $"Tid: {SelectedDateTime:HH:mm}\n\n" +
                "Tack för din bokning och varmt välkommen!"
            );

            // Gå vidare till bekräftelsesidan
            return RedirectToPage("/Bookings/ConfirmationSimple", new { id = booking.Id });
        }

    }
}
