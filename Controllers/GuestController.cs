using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Models;
using CogStayMVC.Enums;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

public class GuestController : Controller
{
    private readonly IGuestService _guestService;
    private readonly IRoomService _roomService;
    private readonly IReservationService _reservationService;
    private readonly IStayRecordService _stayRecordService;
    private readonly IBillingService _billingService;

    public GuestController(
        IGuestService guestService,
        IRoomService roomService,
        IReservationService reservationService,
        IStayRecordService stayRecordService,
        IBillingService billingService)
    {
        _guestService = guestService;
        _roomService = roomService;
        _reservationService = reservationService;
        _stayRecordService = stayRecordService;
        _billingService = billingService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var guest = await _guestService.AuthenticateAsync(email, password);
        if (guest == null)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, guest.FullName),
            new Claim(ClaimTypes.Email, guest.Email),
            new Claim(ClaimTypes.Role, "Guest"),
            new Claim("UserId", guest.GuestId.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string fullName, string email, string phoneNumber, string address, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError("", "Passwords do not match.");
            return View();
        }

        var guest = new Guest
        {
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address
        };

        try
        {
            await _guestService.RegisterAsync(guest, password);
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var guest = await _guestService.GetByIdAsync(guestId);
        if (guest == null)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        // Upcoming reservation logic
        Reservation? upcoming = null;
        foreach (var r in guest.Reservations)
        {
            if (r.ReservationStatus == ReservationStatus.Booked && r.CheckInDate > DateTime.Now)
            {
                if (upcoming == null || r.CheckInDate < upcoming.CheckInDate)
                {
                    upcoming = r;
                }
            }
        }

        // Active Stay logic
        StayRecord? activeStay = null;
        foreach (var s in guest.StayRecords)
        {
            if (s.ActualCheckIn != null && s.ActualCheckOut == null)
            {
                activeStay = s;
                break;
            }
        }

        // Pending Payment details
        decimal pendingAmount = 0;
        Billing? pendingBill = null;
        foreach (var s in guest.StayRecords)
        {
            if (s.Billing != null && s.Billing.PaymentStatus == PaymentStatus.Pending)
            {
                pendingAmount += s.Billing.TotalAmount;
                if (pendingBill == null) pendingBill = s.Billing;
            }
        }

        ViewData["GuestName"] = guest.FullName;
        ViewData["GuestEmail"] = guest.Email;
        ViewData["Upcoming"] = upcoming;
        ViewData["ActiveStay"] = activeStay;
        ViewData["PendingAmount"] = pendingAmount;
        ViewData["PendingBill"] = pendingBill;

        return View();
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> AvailableRooms()
    {
        var rooms = await _roomService.GetAvailableRoomsAsync();
        return View(rooms);
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> BookRoom(int? roomId)
    {
        if (roomId.HasValue)
        {
            var room = await _roomService.GetByIdAsync(roomId.Value);
            ViewData["SelectedRoom"] = room;
        }
        var rooms = await _roomService.GetAvailableRoomsAsync();
        return View(rooms);
    }

    [Authorize(Roles = "Guest")]
    [HttpPost]
    public async Task<IActionResult> BookRoom(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var reservation = new Reservation
        {
            GuestId = guestId,
            RoomId = roomId,
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        try
        {
            await _reservationService.CreateReservationAsync(reservation);
            return RedirectToAction("MyReservations");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var room = await _roomService.GetByIdAsync(roomId);
            ViewData["SelectedRoom"] = room;
            var rooms = await _roomService.GetAvailableRoomsAsync();
            return View(rooms);
        }
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> MyReservations()
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var list = await _reservationService.GetByGuestIdAsync(guestId);
        return View(list);
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> BookingHistory()
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var list = await _reservationService.GetByGuestIdAsync(guestId);
        return View(list);
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> CheckInStatus()
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var guest = await _guestService.GetByIdAsync(guestId);
        
        StayRecord? activeStay = null;
        if (guest != null)
        {
            foreach (var s in guest.StayRecords)
            {
                if (s.ActualCheckIn != null && s.ActualCheckOut == null)
                {
                    activeStay = s;
                    break;
                }
            }
        }
        return View(activeStay);
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> Billing()
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var guest = await _guestService.GetByIdAsync(guestId);
        
        var bills = new List<Billing>();
        if (guest != null)
        {
            foreach (var s in guest.StayRecords)
            {
                if (s.Billing != null)
                {
                    bills.Add(s.Billing);
                }
            }
        }
        return View(bills);
    }

    [Authorize(Roles = "Guest")]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var guest = await _guestService.GetByIdAsync(guestId);
        return View(guest);
    }

    [Authorize(Roles = "Guest")]
    [HttpPost]
    public async Task<IActionResult> Profile(string fullName, string email, string phoneNumber, string address, string? newPassword)
    {
        int guestId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var guest = new Guest
        {
            GuestId = guestId,
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address,
            PasswordHash = newPassword ?? ""
        };

        try
        {
            await _guestService.UpdateProfileAsync(guest);
            ViewData["SuccessMessage"] = "Profile updated successfully.";
            
            // Re-sign in to update name claim if it changed
            var identity = (ClaimsIdentity?)User.Identity;
            if (identity != null)
            {
                var claim = identity.FindFirst(ClaimTypes.Name);
                if (claim != null) identity.RemoveClaim(claim);
                identity.AddClaim(new Claim(ClaimTypes.Name, fullName));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        var dbGuest = await _guestService.GetByIdAsync(guestId);
        return View(dbGuest);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
