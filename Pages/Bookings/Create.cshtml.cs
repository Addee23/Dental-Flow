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
      

        public CreateModel(AppDbContext context,
      UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // Bindningar
        [BindProperty] public Booking Booking { get; set; }
        [BindProperty] public string SelectedTime { get; set; }
        [BindProperty] public string BookingDate { get; set; }

        public List<DateTime> Dates { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
        public Service Service { get; set; }

        // GET
        public async Task<IActionResult> OnGet(int serviceId)
        {
            Service = _context.Services.FirstOrDefault(s => s.Id == serviceId);
            if (Service == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            Booking = new Booking
            {
                ServiceId = Service.Id,
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Phone = user.PhoneNumber
            };

            // Generera 60 dagar framåt
            Dates = Enumerable.Range(0, 60)
                .Select(i => DateTime.Today.AddDays(i))
                .ToList();

            // Sätt dagens datum som default
            if (string.IsNullOrEmpty(BookingDate))
                BookingDate = DateTime.Today.ToString("yyyy-MM-dd");

            GenerateTimeSlots();

            return Page();
        }

        private void GenerateTimeSlots()
        {
            var selectedDate = DateTime.Parse(BookingDate);
            var now = DateTime.Now;

            TimeSlots = Enumerable.Range(0, 10)  // 08–17
                .Select(i => new TimeSlot
                {
                    Time = $"{8 + i:00}:00",
                    IsPast = selectedDate.Date == now.Date && (8 + i) <= now.Hour
                })
                .ToList();
        }

        // POST (boka tid)
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("CREATE MODEL POST KÖRS");

            var now = DateTime.Now;

            if (string.IsNullOrWhiteSpace(SelectedTime) ||
                string.IsNullOrWhiteSpace(BookingDate))
            {
                ModelState.AddModelError("", "Du måste välja både datum och tid.");
                GenerateTimeSlots();
                return Page();
            }

            if (!DateTime.TryParse($"{BookingDate} {SelectedTime}", out var fullDateTime))
            {
                ModelState.AddModelError("", "Ogiltigt datum eller tid.");
                GenerateTimeSlots();
                return Page();
            }

            if (fullDateTime <= now)
            {
                ModelState.AddModelError("", "Du kan inte boka en tid som har passerat.");
                GenerateTimeSlots();
                return Page();
            }

            Booking.DateTime = fullDateTime;
            Booking.CreatedAt = now;
            Booking.UserId = _userManager.GetUserId(User);

            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            Booking.Service = _context.Services.FirstOrDefault(s => s.Id == Booking.ServiceId);


            return RedirectToPage("ConfirmationSimple", new { id = Booking.Id });
        }

        public class TimeSlot
        {
            public string Time { get; set; }
            public bool IsPast { get; set; }
        }
    }
}
