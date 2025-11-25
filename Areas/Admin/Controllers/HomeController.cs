using DentalFlow.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .OrderByDescending(b => b.DateTime)
                .ToListAsync();

            ViewBag.TotalBookings = bookings.Count;
            ViewBag.TotalServices = _context.Services.Count();
            ViewBag.Upcoming = bookings.Count(b => b.DateTime > DateTime.Now);

            return View();
        }
    }
}
