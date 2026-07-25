using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Controller for managing housekeeping operations. Allows housekeeping staff and managers
/// to assign, view, update, and delete cleaning tasks, synchronizing room statuses (e.g. Dirty/Available).
/// </summary>
public class HousekeepingController : Controller
{
    private readonly IHousekeepingService _housekeepingService;
    private readonly IRoomService _roomService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HousekeepingController"/> class.
    /// </summary>
    /// <param name="housekeepingService">Service managing housekeeping tasks.</param>
    /// <param name="roomService">Service managing room records.</param>
    public HousekeepingController(
        IHousekeepingService housekeepingService,
        IRoomService roomService)
    {
        _housekeepingService = housekeepingService;
        _roomService = roomService;
    }

    /// <summary>
    /// Lists all housekeeping tasks. Restricted to Housekeeping or Manager roles.
    /// </summary>
    /// <returns>Index View displaying tasks.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Housekeeping" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var tasks = await _housekeepingService.GetAllTasksAsync();
        return View(tasks);
    }

    /// <summary>
    /// Displays details of a specific housekeeping task.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <returns>Details View containing task details, or 404.</returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Housekeeping" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var task = await _housekeepingService.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    /// <summary>
    /// Renders the housekeeping task creation form.
    /// </summary>
    /// <returns>Create task form View.</returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Housekeeping" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        ViewBag.Rooms = await _roomService.GetAllRoomsAsync();
        return View(new CreateHousekeepingTaskDTO());
    }

    /// <summary>
    /// Processes task creation submissions.
    /// </summary>
    /// <param name="dto">Create task details DTO.</param>
    /// <returns>Redirects back to task list on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateHousekeepingTaskDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Housekeeping" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        if (!ModelState.IsValid)
        {
            ViewBag.Rooms = await _roomService.GetAllRoomsAsync();
            return View(dto);
        }

        try
        {
            await _housekeepingService.CreateTaskAsync(dto);
            TempData["Success"] = "Housekeeping cleaning request created.";
            return RedirectToAction(nameof(Index), new { role = staffRole });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Rooms = await _roomService.GetAllRoomsAsync();
            return View(dto);
        }
    }

    /// <summary>
    /// Renders the edit status form for a housekeeping task.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <returns>Edit form View displaying task status details.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Housekeeping" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var task = await _housekeepingService.GetTaskByIdAsync(id);

        if (task == null)
        {
            return Content($"Task with ID {id} was not found in the database.");
        }

        var dto = new UpdateTaskStatusDTO
        {
            TaskId = task.TaskId,
            TaskStatus = task.TaskStatus
        };
        ViewBag.Task = task;
        return View(dto);
    }

    /// <summary>
    /// Processes task status updates and syncs room availability back to Front Desk.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <param name="dto">Update status details DTO containing new status.</param>
    /// <returns>Redirects back to task Index on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateTaskStatusDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Housekeeping" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        if (id != dto.TaskId) return BadRequest();

        if (!ModelState.IsValid)
        {
            ViewBag.Task = await _housekeepingService.GetTaskByIdAsync(dto.TaskId);
            return View(dto);
        }

        try
        {
            await _housekeepingService.UpdateTaskStatusAsync(dto);
            TempData["Success"] = "Task status updated! Room status synchronized.";
            return RedirectToAction(nameof(Index), new { role = staffRole });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Task = await _housekeepingService.GetTaskByIdAsync(dto.TaskId);
            return View(dto);
        }
    }

    /// <summary>
    /// Deletes a specific housekeeping task.
    /// </summary>
    /// <param name="id">Task identifier.</param>
    /// <returns>Redirects back to task Index.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "Housekeeping" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        try
        {
            await _housekeepingService.DeleteTaskAsync(id);
            TempData["Success"] = "Housekeeping task deleted.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { role = staffRole });
    }
}
