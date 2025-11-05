using DentalFlow.Data;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DentalFlow.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int ServiceId { get; set; }

        public void OnGet(int serviceId)
        {
            ServiceId = serviceId;
        }
    }
}
