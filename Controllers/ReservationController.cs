using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Models;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

[Authorize(Roles = "Admin,Manager,FrontDesk")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IRoomService _roomService;
    private readonly IGuestService _guestService;

    public ReservationController(
        IReservationService reservationService,
        IRoomService roomService,
        IGuestService guestService)
    {
        _reservationService = reservationService;
        _roomService = roomService;
        _guestService = guestService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? searchString)
    {
        var list = await _reservationService.SearchReservationsAsync(searchString);
        ViewData["SearchString"] = searchString;
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);
        if (reservation == null) return NotFound();
        return View(reservation);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["AvailableRooms"] = await _roomService.GetAvailableRoomsAsync();
        ViewData["Guests"] = await _guestService.GetAllGuestsAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Reservation reservation)
    {
        try
        {
            await _reservationService.CreateReservationAsync(reservation);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["AvailableRooms"] = await _roomService.GetAvailableRoomsAsync();
            ViewData["Guests"] = await _guestService.GetAllGuestsAsync();
            return View(reservation);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            await _reservationService.CancelReservationAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Details", new { id = id });
        }
    }
}
