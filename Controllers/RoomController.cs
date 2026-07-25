using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Controller for managing rooms. Handles room creation, modification, deletion,
/// detail queries, and room availability status checks for admin, managers, and front desk staff.
/// </summary>
public class RoomController : Controller
{
    private readonly IRoomService _roomService;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoomController"/> class.
    /// </summary>
    /// <param name="roomService">Service handling room operations and queries.</param>
    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /// <summary>
    /// Displays a list of all hotel rooms. Restricted to Admin and Manager roles.
    /// </summary>
    /// <returns>Index View displaying rooms.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Admin" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var rooms = await _roomService.GetAllRoomsAsync();
        return View(rooms);
    }

    /// <summary>
    /// Displays detailed info for a specific room. Restricted to Admin and Manager roles.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    /// <returns>Details View, or 404 NotFound.</returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Admin" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null) return NotFound();
        return View(room);
    }

    /// <summary>
    /// Renders the room creation form view. Restricted to Admin and Manager roles.
    /// </summary>
    /// <returns>Create room form View.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Admin" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        return View(new CreateRoomDTO());
    }

    /// <summary>
    /// Processes room creation form submissions.
    /// </summary>
    /// <param name="dto">Create room details DTO.</param>
    /// <returns>Redirects to room list view on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoomDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Admin" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _roomService.CreateRoomAsync(dto);
            TempData["Success"] = "Room created successfully!";
            return RedirectToAction(nameof(Index), new { role = staffRole });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Renders the room edit form view. Restricted to Admin and Manager roles.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    /// <returns>Edit form View containing room details, or 404.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Admin" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var room = await _roomService.GetRoomByIdAsync(id);
        if (room == null) return NotFound();

        var dto = new UpdateRoomDTO
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            PricePerNight = room.PricePerNight,
            Status = room.Status
        };
        return View(dto);
    }

    /// <summary>
    /// Processes room update submissions.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    /// <param name="dto">Updated room details DTO.</param>
    /// <returns>Redirects to room list view on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateRoomDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Admin" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        if (id != dto.RoomId) return BadRequest();
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _roomService.UpdateRoomAsync(dto);
            TempData["Success"] = "Room updated successfully!";
            return RedirectToAction(nameof(Index), new { role = staffRole });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Deletes a specific room. Restricted to Admin and Manager roles.
    /// </summary>
    /// <param name="id">Room identifier.</param>
    /// <returns>Redirects back to Index action.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Admin" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        try
        {
            await _roomService.DeleteRoomAsync(id);
            TempData["Success"] = "Room deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { role = staffRole });
    }

    /// <summary>
    /// Displays rooms currently available for checkout or assignment.
    /// Restricted to FrontDesk, Manager, or Admin staff.
    /// </summary>
    /// <returns>CheckAvailability View showing available rooms.</returns>
    [HttpGet]
    public async Task<IActionResult> CheckAvailability()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager" && staffRole != "Admin"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var availableRooms = await _roomService.GetAvailableRoomsAsync();
        return View(availableRooms);
    }
}
