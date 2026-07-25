using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Repositories.Implementations;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;
using CogStayMVC.Repositories.Admin;
using CogStayMVC.Repositories.FrontDesk;
using CogStayMVC.Repositories.GuestModule;
using CogStayMVC.Repositories.Housekeeping;
using CogStayMVC.Repositories.Manager;
using CogStayMVC.Services.Admin;
using CogStayMVC.Services.FrontDesk;
using CogStayMVC.Services.GuestModule;
using CogStayMVC.Services.Housekeeping;
using CogStayMVC.Services.Manager;

// Create the WebApplication builder instance.
// This sets up configuration, logging, default services, and environments.
var builder = WebApplication.CreateBuilder(args);

// Add standard ASP.NET Core MVC controllers with views services.
// Configures services for routing, model binding, rendering Razor views, etc.
builder.Services.AddControllersWithViews();

// Register the Entity Framework database context using SQL Server provider.
// Configures it to pull the connection string named "DefaultConnection" from appsettings.json.
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Session State services.
// Registers memory caching needed for session storage and options for timeout, HTTP-only cookie, and essential flags.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Sessions expire after 60 minutes of inactivity
    options.Cookie.HttpOnly = true;                 // Secure cookie from client-side script access
    options.Cookie.IsEssential = true;              // Required for proper site operation under GDPR
});

// --- Register Repositories (Data Access Layer) ---
// Scoped lifetimes are used here; EF DbContext is scoped, so repository instances live per request.
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IStayRecordRepository, StayRecordRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<IHousekeepingTaskRepository, HousekeepingTaskRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();

// --- Register Services (Business Logic Layer) ---
// Business services coordinates validation, status checks, mapping entities to DTOs, and invoking repository updates.
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IHousekeepingService, HousekeepingService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

// Build the WebApplication pipeline.
var app = builder.Build();

// Configure the HTTP request middleware pipeline.
if (!app.Environment.IsDevelopment())
{
    // Production settings: custom error page and HSTS security headers.
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS secure URLs.
app.UseHttpsRedirection();

// Enables static file serving (CSS, JS, Images) from the wwwroot folder.
app.UseStaticFiles();

// Configures request routing matching to map URLs to controller actions.
app.UseRouting();

// Enable session state middleware before authorization checks.
app.UseSession();

// Enables authorization rules evaluation for requests.
app.UseAuthorization();

// Setup the default MVC route pattern mapping.
// Defaults to the HomeController Index action if no controller/action is specified.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Starts listening for incoming HTTP requests.
app.Run();
