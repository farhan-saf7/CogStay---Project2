using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Data;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services;
using CogStayMVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Configuration (Fallback to In-Memory if specified in appsettings.json)
var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
if (useInMemory)
{
    builder.Services.AddDbContext<HotelDbContext>(options =>
        options.UseInMemoryDatabase("CogStayDb"));
}
else
{
    builder.Services.AddDbContext<HotelDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// Authentication Setup
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = async context =>
        {
            var queryRole = context.Request.Query["role"].ToString();
            
            // Check ReturnUrl if role query is missing
            if (string.IsNullOrEmpty(queryRole) && context.Request.Query.TryGetValue("ReturnUrl", out var returnUrl))
            {
                var returnUrlStr = returnUrl.ToString();
                if (returnUrlStr.Contains("role="))
                {
                    var parsed = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(returnUrlStr);
                    if (parsed.TryGetValue("role", out var r))
                    {
                        queryRole = r.ToString();
                    }
                    else if (returnUrlStr.Contains("role=FrontDesk")) queryRole = "FrontDesk";
                    else if (returnUrlStr.Contains("role=Housekeeping")) queryRole = "Housekeeping";
                    else if (returnUrlStr.Contains("role=Manager")) queryRole = "Manager";
                    else if (returnUrlStr.Contains("role=Admin")) queryRole = "Admin";
                    else if (returnUrlStr.Contains("role=Guest")) queryRole = "Guest";
                }
            }

            if (!string.IsNullOrEmpty(queryRole))
            {
                // Auto login simulation during development
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, $"Demo {queryRole}"),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, $"{queryRole.ToLower()}@cogstay.com"),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, queryRole),
                    new System.Security.Claims.Claim("UserId", "1") // Mock ID
                };
                var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                await context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                
                context.Response.Redirect(context.RedirectUri);
                return;
            }

            // Redirect based on prefix
            if (context.Request.Path.StartsWithSegments("/Guest"))
            {
                context.Response.Redirect("/Guest/Login");
            }
            else
            {
                context.Response.Redirect("/Staff/Login");
            }
        };
    });

// Dependency Injection: Repositories
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IStayRecordRepository, StayRecordRepository>();
builder.Services.AddScoped<IHousekeepingRepository, HousekeepingRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();

// Dependency Injection: Services
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IStayRecordService, StayRecordService>();
builder.Services.AddScoped<IHousekeepingService, HousekeepingService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IStaffService, StaffService>();

var app = builder.Build();

// Seed Database records
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    if (useInMemory)
    {
        context.Database.EnsureCreated();
    }
    await SeedData(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Database Seeder
static async Task SeedData(HotelDbContext context)
{
    // Seed Guest Account
    if (!await context.Guests.AnyAsync())
    {
        var guest = new Guest
        {
            FullName = "Demo Guest",
            Email = "guest@example.com",
            PhoneNumber = "+91 99999-88888",
            Address = "Crescent Enclave, New Delhi, India",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("guest123"),
            CreatedAt = DateTime.UtcNow
        };
        await context.Guests.AddAsync(guest);
    }

    // Seed Staff Roles
    if (!await context.Staff.AnyAsync())
    {
        var staffList = new List<Staff>
        {
            new Staff
            {
                FullName = "Admin Administrator",
                Email = "admin@cogstay.com",
                PhoneNumber = "+91 91111-22222",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = StaffRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Staff
            {
                FullName = "Vikram Malhotra",
                Email = "manager@cogstay.com",
                PhoneNumber = "+91 97777-55555",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
                Role = StaffRole.Manager,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Staff
            {
                FullName = "Alexander Pierce",
                Email = "frontdesk@cogstay.com",
                PhoneNumber = "+91 98888-77777",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("frontdesk123"),
                Role = StaffRole.FrontDesk,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Staff
            {
                FullName = "Jane Henderson",
                Email = "housekeeping@cogstay.com",
                PhoneNumber = "+91 98888-66666",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("housekeeping123"),
                Role = StaffRole.Housekeeping,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };
        await context.Staff.AddRangeAsync(staffList);
    }

    // Seed Room inventory
    if (!await context.Rooms.AnyAsync())
    {
        var rooms = new List<Room>
        {
            new Room { RoomNumber = "101", RoomType = "Classic Superior King", PricePerNight = 11200.00m, Status = RoomStatus.Available },
            new Room { RoomNumber = "205", RoomType = "Deluxe Executive Room", PricePerNight = 14400.00m, Status = RoomStatus.Available },
            new Room { RoomNumber = "304", RoomType = "Luxury Penthouse Suite", PricePerNight = 28000.00m, Status = RoomStatus.UnderMaintenance }
        };
        await context.Rooms.AddRangeAsync(rooms);
    }

    await context.SaveChangesAsync();
}
