using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DentalFlow.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public List<Service> Services { get; set; } = new();

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Services = _context.Services
                .OrderBy(s => s.DurationMinutes)
                .ToList();
        }
    }
}
