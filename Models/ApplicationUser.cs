using Microsoft.AspNetCore.Identity;

namespace DentalFlow.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
