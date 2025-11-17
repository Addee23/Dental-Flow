using DentalFlow.Data;
using DentalFlow.Data.Seed;
using DentalFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB & Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(o =>
{
    o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();


// Migrate + seed services
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// seed roles + admin
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await DentalFlow.Data.Seed.RoleSeeder.SeedRolesAsync(roleManager);
    await DentalFlow.Data.Seed.AdminSeeder.SeedAdminAsync(userManager);
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Services.Any())
    {
        db.Services.AddRange(
            new Service { Name = "Tandundersökning", DurationMinutes = 45, Price = 850 },
            new Service { Name = "Tandblekning", DurationMinutes = 60, Price = 950 },
            new Service { Name = "Akutbesök", DurationMinutes = 30, Price = 650 }
        );
        db.SaveChanges();
    }
}

app.Run();
