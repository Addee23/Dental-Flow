using DentalFlow.Data;
using DentalFlow.Models;
using DentalFlow.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public CustomerController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MinaBokningar()
        {
            var user = await _userManager.GetUserAsync(User);

            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.UserId == user.Id)
                .OrderByDescending(b => b.DateTime)
                .ToListAsync();

            return View(bookings);
        }

        public IActionResult Bookings()
        {
            var userId = _userManager.GetUserId(User);

            var bookings = _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.DateTime)
                .ToList();

            return View("MinaBokningar", bookings);
        }


        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            if (booking.UserId != _userManager.GetUserId(User))
                return Unauthorized();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("MinaBokningar");
        }
    }
}
