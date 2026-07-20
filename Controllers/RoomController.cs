using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Models;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

[Authorize]
public class RoomController : Controller
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IActionResult> Index(string? searchString)
    {
        var rooms = await _roomService.SearchRoomsAsync(searchString);
        ViewData["SearchString"] = searchString;
        return View(rooms);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null) return NotFound();
        return View(room);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(Room room)
    {
        try
        {
            await _roomService.CreateRoomAsync(room);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(room);
        }
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null) return NotFound();
        return View(room);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Room room)
    {
        room.RoomId = id;
        try
        {
            await _roomService.UpdateRoomAsync(room);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var dbRoom = await _roomService.GetByIdAsync(id);
            return View(dbRoom);
        }
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _roomService.DeleteRoomAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Details", new { id = id });
        }
    }

    [Authorize(Roles = "Admin,Manager,FrontDesk")]
    [HttpGet]
    public async Task<IActionResult> CheckAvailability()
    {
        var rooms = await _roomService.GetAvailableRoomsAsync();
        return View(rooms);
    }
}
