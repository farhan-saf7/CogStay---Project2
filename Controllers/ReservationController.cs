using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Controller handling booking reservations. It handles listing all reservations,
/// checking details, manual booking creation for front desk/managers, cancellations, and reservation removals.
/// </summary>
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IRoomService _roomService;
    private readonly IGuestService _guestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationController"/> class.
    /// </summary>
    /// <param name="reservationService">Service for handling room reservations.</param>
    /// <param name="roomService">Service for handling room updates.</param>
    /// <param name="guestService">Service querying guest data.</param>
    public ReservationController(
        IReservationService reservationService,
        IRoomService roomService,
        IGuestService guestService)
    {
        _reservationService = reservationService;
        _roomService = roomService;
        _guestService = guestService;
    }

    /// <summary>
    /// Displays all guest reservations. Restricted to FrontDesk or Manager roles.
    /// </summary>
    /// <returns>Index View displaying reservations list.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var reservations = await _reservationService.GetAllReservationsAsync();
        return View(reservations);
    }

    /// <summary>
    /// Displays details of a specific reservation. Restricted to FrontDesk or Manager roles.
    /// </summary>
    /// <param name="id">Reservation ID.</param>
    /// <returns>Details View containing reservation particulars, or NotFound.</returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null) return NotFound();
        return View(reservation);
    }

    /// <summary>
    /// Renders the manual booking creation view.
    /// </summary>
    /// <returns>Create reservation form View.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        ViewBag.Guests = await _guestService.GetAllGuestsAsync();
        ViewBag.Rooms = await _roomService.GetAvailableRoomsAsync();
        return View(new CreateReservationDTO
        {
            CheckInDate = DateTime.Today,
            CheckOutDate = DateTime.Today.AddDays(1)
        });
    }

    /// <summary>
    /// Processes manually entered reservations from Front Desk or Manager.
    /// </summary>
    /// <param name="dto">The reservation details DTO.</param>
    /// <returns>Redirects back to reservation Index on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReservationDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        if (!ModelState.IsValid)
        {
            ViewBag.Guests = await _guestService.GetAllGuestsAsync();
            ViewBag.Rooms = await _roomService.GetAvailableRoomsAsync();
            return View(dto);
        }

        try
        {
            await _reservationService.BookRoomAsync(dto);
            TempData["Success"] = "Reservation created successfully!";
            return RedirectToAction(nameof(Index), new { role = staffRole });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Guests = await _guestService.GetAllGuestsAsync();
            ViewBag.Rooms = await _roomService.GetAvailableRoomsAsync();
            return View(dto);
        }
    }

    /// <summary>
    /// Cancels a specific booked reservation.
    /// </summary>
    /// <param name="id">Reservation ID.</param>
    /// <returns>Redirects back to reservation Index View.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        try
        {
            await _reservationService.CancelReservationAsync(id);
            TempData["Success"] = "Reservation cancelled.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { role = staffRole });
    }

    /// <summary>
    /// Deletes a reservation record from the database.
    /// </summary>
    /// <param name="id">Reservation ID.</param>
    /// <returns>Redirects back to reservation Index View.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        try
        {
            await _reservationService.DeleteReservationAsync(id);
            TempData["Success"] = "Reservation deleted.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { role = staffRole });
    }
}
