using System.Linq;
using System.Threading.Tasks;
using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Bookings
      
        public async Task<IActionResult> Index(DateTime? date, int? month)
        {
            var query = _context.Bookings
                .Include(b => b.Service)
                .AsQueryable();

            // 🔹 Filtrering på specifikt datum
            if (date.HasValue)
            {
                var selectedDate = date.Value.Date;
                query = query.Where(b => b.DateTime.Date == selectedDate);
            }

            // 🔹 Filtrering på månad
            if (month.HasValue && month.Value >= 1 && month.Value <= 12)
            {
                query = query.Where(b => b.DateTime.Month == month.Value);
            }

            var bookings = await query
                .OrderByDescending(b => b.DateTime)
                .ToListAsync();

            ViewBag.SelectedDate = date?.ToString("yyyy-MM-dd");
            ViewBag.SelectedMonth = month;

            return View(bookings);
        }



        // GET: Admin/Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Admin/Bookings/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: Admin/Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CreatedAt = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", booking.ServiceId);
            return View(booking);
        }

        // GET: Admin/Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", booking.ServiceId);
            return View(booking);
        }

        // POST: Admin/Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Bookings.Any(e => e.Id == booking.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", booking.ServiceId);
            return View(booking);
        }

        // GET: Admin/Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Admin/Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
