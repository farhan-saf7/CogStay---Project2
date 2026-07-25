using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Controller for processing guest arrivals and departures. Coordinates guest check-in operations
/// and redirects departing guests to the billing pipeline to settle their payments.
/// </summary>
public class CheckInController : Controller
{
    private readonly ICheckInService _checkInService;
    private readonly IReservationService _reservationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckInController"/> class.
    /// </summary>
    /// <param name="checkInService">Service managing check-in activations and check-out workflows.</param>
    /// <param name="reservationService">Service query reservations to map during guest check-in.</param>
    public CheckInController(
        ICheckInService checkInService,
        IReservationService reservationService)
    {
        _checkInService = checkInService;
        _reservationService = reservationService;
    }

    /// <summary>
    /// Lists all current and active stays. Restricted to FrontDesk staff.
    /// </summary>
    /// <returns>ActiveStays View displaying active stay records.</returns>
    [HttpGet]
    public async Task<IActionResult> ActiveStays()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "FrontDesk")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "FrontDesk";
        var stays = await _checkInService.GetAllStaysAsync();
        return View(stays);
    }

    /// <summary>
    /// Renders the check-in registration form for active room bookings.
    /// </summary>
    /// <returns>CheckIn View displaying confirmed reservations list.</returns>
    [HttpGet]
    public async Task<IActionResult> CheckIn()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "FrontDesk")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "FrontDesk";
        ViewBag.Reservations = await _reservationService.GetAllReservationsAsync();
        return View(new CreateCheckInDTO());
    }

    /// <summary>
    /// Processes the guest check-in submission and updates room availability to Occupied.
    /// </summary>
    /// <param name="dto">The check-in DTO linking to a booked reservation.</param>
    /// <returns>Redirects to ActiveStays View on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckIn(CreateCheckInDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "FrontDesk")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "FrontDesk";
        if (!ModelState.IsValid)
        {
            ViewBag.Reservations = await _reservationService.GetAllReservationsAsync();
            return View(dto);
        }

        try
        {
            await _checkInService.CheckInGuestAsync(dto);
            TempData["Success"] = "Guest checked in successfully! Room status updated to Occupied.";
            return RedirectToAction(nameof(ActiveStays), new { role = "FrontDesk" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Reservations = await _reservationService.GetAllReservationsAsync();
            return View(dto);
        }
    }

    /// <summary>
    /// Renders the check-out confirmation page for an active stay record.
    /// </summary>
    /// <param name="id">Optional stay record ID to preselect.</param>
    /// <returns>CheckOut View displaying active guests.</returns>
    [HttpGet]
    public async Task<IActionResult> CheckOut(int? id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "FrontDesk")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "FrontDesk";
        var stays = await _checkInService.GetAllStaysAsync();
        ViewBag.Stays = stays;
        return View(new CheckOutDTO { StayId = id ?? 0 });
    }

    /// <summary>
    /// Initiates guest check-out and redirects to Billing payment action.
    /// </summary>
    /// <param name="dto">The checkout DTO details containing target stay reference.</param>
    /// <returns>Redirects to Payment action in Billing controller.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(CheckOutDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "FrontDesk")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "FrontDesk";
        if (!ModelState.IsValid)
        {
            ViewBag.Stays = await _checkInService.GetAllStaysAsync();
            return View(dto);
        }

        try
        {
            // Front Desk initiates checkout -> redirects to Front Desk Billing Module for final payment & cleaning request creation!
            return RedirectToAction("Payment", "Billing", new { stayId = dto.StayId, role = "FrontDesk" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Stays = await _checkInService.GetAllStaysAsync();
            return View(dto);
        }
    }

    /// <summary>
    /// Deletes a specific stay record.
    /// </summary>
    /// <param name="id">The ID of the stay record to delete.</param>
    /// <returns>Redirects back to ActiveStays View.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "FrontDesk")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "FrontDesk";
        try
        {
            await _checkInService.DeleteStayAsync(id);
            TempData["Success"] = "Stay record deleted.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(ActiveStays), new { role = "FrontDesk" });
    }
}
