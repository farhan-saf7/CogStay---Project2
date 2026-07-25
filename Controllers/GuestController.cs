using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Controller for guest operations. Handles registration, authentication, room discovery,
/// reservation booking, personal bill queries, profile updates, and logout actions.
/// </summary>
public class GuestController : Controller
{
    private readonly IGuestService _guestService;
    private readonly IRoomService _roomService;
    private readonly IReservationService _reservationService;
    private readonly ICheckInService _checkInService;
    private readonly IBillingService _billingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GuestController"/> class.
    /// </summary>
    /// <param name="guestService">Service handling guest CRUD and validation.</param>
    /// <param name="roomService">Service managing hotel room views.</param>
    /// <param name="reservationService">Service handling booking logic and checks.</param>
    /// <param name="checkInService">Service managing check-in stay records.</param>
    /// <param name="billingService">Service handling billing invoice queries.</param>
    public GuestController(
        IGuestService guestService,
        IRoomService roomService,
        IReservationService reservationService,
        ICheckInService checkInService,
        IBillingService billingService)
    {
        _guestService = guestService;
        _roomService = roomService;
        _reservationService = reservationService;
        _checkInService = checkInService;
        _billingService = billingService;
    }

    /// <summary>
    /// Renders the guest login view.
    /// </summary>
    /// <returns>Login form View.</returns>
    [HttpGet]
    public IActionResult Login() => View();

    /// <summary>
    /// Processes guest login credentials and sets up session variables.
    /// </summary>
    /// <param name="dto">Data transfer object containing guest login credentials.</param>
    /// <returns>Redirects to Guest Dashboard on success, or returns form on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(GuestLoginDTO dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var guest = await _guestService.ValidateGuestLoginAsync(dto);
        if (guest == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(dto);
        }

        HttpContext.Session.SetInt32("GuestId", guest.GuestId);
        HttpContext.Session.SetString("GuestName", guest.FullName);
        HttpContext.Session.SetString("GuestEmail", guest.Email);

        return RedirectToAction(nameof(Dashboard));
    }

    /// <summary>
    /// Renders the guest registration form.
    /// </summary>
    /// <returns>Registration form View.</returns>
    [HttpGet]
    public IActionResult Register() => View();

    /// <summary>
    /// Processes new guest registration and sets up their active session.
    /// </summary>
    /// <param name="dto">The guest registration details DTO.</param>
    /// <returns>Redirects to Guest Dashboard on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CreateGuestDTO dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            var guest = await _guestService.RegisterGuestAsync(dto);
            HttpContext.Session.SetInt32("GuestId", guest.GuestId);
            HttpContext.Session.SetString("GuestName", guest.FullName);
            HttpContext.Session.SetString("GuestEmail", guest.Email);

            TempData["Success"] = "Account registered successfully!";
            return RedirectToAction(nameof(Dashboard));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Displays the Guest Dashboard summarizing their active reservations.
    /// </summary>
    /// <returns>Dashboard View displaying current guest reservations.</returns>
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));

        var guest = await _guestService.GetGuestByIdAsync(guestId.Value);
        if (guest == null) return RedirectToAction(nameof(Login));

        ViewBag.GuestName = guest.FullName;
        HttpContext.Session.SetString("GuestName", guest.FullName);
        HttpContext.Session.SetString("GuestEmail", guest.Email);

        var reservations = await _reservationService.GetReservationsByGuestAsync(guestId.Value);
        return View(reservations);
    }

    /// <summary>
    /// Displays all rooms that are currently available for booking.
    /// </summary>
    /// <returns>AvailableRooms View containing room lists.</returns>
    [HttpGet]
    public async Task<IActionResult> AvailableRooms()
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));

        ViewData["Role"] = "Guest";
        var rooms = await _roomService.GetAvailableRoomsAsync();
        return View(rooms);
    }

    /// <summary>
    /// Renders the booking page for a specific room.
    /// </summary>
    /// <param name="roomId">Optional room identifier to pre-select.</param>
    /// <returns>BookRoom View containing reservation setup details.</returns>
    [HttpGet]
    public async Task<IActionResult> BookRoom(int? roomId)
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));

        ViewBag.AvailableRooms = await _roomService.GetAvailableRoomsAsync();
        var dto = new CreateReservationDTO
        {
            GuestId = guestId.Value,
            RoomId = roomId ?? 0,
            CheckInDate = DateTime.Today,
            CheckOutDate = DateTime.Today.AddDays(1)
        };
        return View(dto);
    }

    /// <summary>
    /// Processes room booking requests from the guest.
    /// </summary>
    /// <param name="dto">The reservation details DTO.</param>
    /// <returns>Redirects to MyReservations View on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookRoom(CreateReservationDTO dto)
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));
        dto.GuestId = guestId.Value;

        if (!ModelState.IsValid)
        {
            ViewBag.AvailableRooms = await _roomService.GetAvailableRoomsAsync();
            return View(dto);
        }

        try
        {
            await _reservationService.BookRoomAsync(dto);
            TempData["Success"] = "Room booked successfully!";
            return RedirectToAction(nameof(MyReservations));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.AvailableRooms = await _roomService.GetAvailableRoomsAsync();
            return View(dto);
        }
    }

    /// <summary>
    /// Displays all active reservations for the current guest.
    /// </summary>
    /// <returns>MyReservations View.</returns>
    [HttpGet]
    public async Task<IActionResult> MyReservations()
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));

        var reservations = await _reservationService.GetReservationsByGuestAsync(guestId.Value);
        return View(reservations);
    }

    /// <summary>
    /// Displays historical reservations completed or cancelled by this guest.
    /// </summary>
    /// <returns>BookingHistory View.</returns>
    [HttpGet]
    public async Task<IActionResult> BookingHistory()
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));

        var reservations = await _reservationService.GetReservationsByGuestAsync(guestId.Value);
        return View(reservations);
    }

    /// <summary>
    /// Lists all invoices and bills associated with the guest.
    /// </summary>
    /// <returns>Billing View displaying guest's invoice list.</returns>
    [HttpGet]
    public async Task<IActionResult> Billing()
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));

        var bills = await _billingService.GetAllBillsAsync();
        return View(bills);
    }

    /// <summary>
    /// Renders the profile update page for the logged-in guest.
    /// </summary>
    /// <returns>Profile View containing guest details.</returns>
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));

        var guest = await _guestService.GetGuestByIdAsync(guestId.Value);
        if (guest == null) return NotFound();

        var dto = new UpdateGuestDTO
        {
            GuestId = guest.GuestId,
            FullName = guest.FullName,
            Email = guest.Email,
            PhoneNumber = guest.PhoneNumber,
            Address = guest.Address
        };
        return View(dto);
    }

    /// <summary>
    /// Processes guest personal profile updates.
    /// </summary>
    /// <param name="dto">The updated guest details DTO.</param>
    /// <returns>Redirects back to Profile page on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(UpdateGuestDTO dto)
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction(nameof(Login));
        dto.GuestId = guestId.Value;

        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _guestService.UpdateGuestAsync(dto);
            HttpContext.Session.SetString("GuestName", dto.FullName);
            HttpContext.Session.SetString("GuestEmail", dto.Email);
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Profile));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Logs the guest out and clears their active session context.
    /// </summary>
    /// <returns>Redirects to guest Login page.</returns>
    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
