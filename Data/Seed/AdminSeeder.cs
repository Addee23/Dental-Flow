using DentalFlow.Models;
using Microsoft.AspNetCore.Identity;
using DentalFlow.Models.Auth;


namespace DentalFlow.Data.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            const string email = "admin@dentalflow.local";
            const string password = "Admin123!";

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Adminsson"

                };
                await userManager.CreateAsync(user, password);
            }

            if (!await userManager.IsInRoleAsync(user, "Admin"))
                await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}
