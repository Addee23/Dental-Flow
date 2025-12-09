using DentalFlow.Data;
using DentalFlow.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentalFlow.Data.Seed;
using DentalFlow.Models;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


// -----------------------------
// Database + Identity
// -----------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EmailService>();



// Session MUST be enabled before building the app
builder.Services.AddSession();

// Avoid implicit required errors
builder.Services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(o =>
{
    o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

// -----------------------------
// Database migration + seeding
// -----------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    SeedData.Initialize(db);
}

// -----------------------------
// Middleware pipeline
// -----------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✔ FIX: Session MUST COME BEFORE Authentication/Authorization
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Admin route
// Admin route – måste ligga först
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Razor Pages routing
app.MapRazorPages();

// Seed roles + admin user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await RoleSeeder.SeedRolesAsync(roleManager);
    await AdminSeeder.SeedAdminAsync(userManager);
}

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Customer"))
    {
        await roleManager.CreateAsync(new IdentityRole("Customer"));
    }
}


app.Run();
