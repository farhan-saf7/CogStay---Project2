using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

[Authorize(Roles = "Admin,Manager,Housekeeping")]
public class HousekeepingController : Controller
{
    private readonly IHousekeepingService _housekeepingService;
    private readonly IRoomService _roomService;

    public HousekeepingController(IHousekeepingService housekeepingService, IRoomService roomService)
    {
        _housekeepingService = housekeepingService;
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tasks = await _housekeepingService.GetAllTasksAsync();
        return View(tasks);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var task = await _housekeepingService.GetByIdAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["Rooms"] = await _roomService.GetAllRoomsAsync();
        return View();
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<IActionResult> Create(HousekeepingTask task)
    {
        try
        {
            await _housekeepingService.AssignTaskAsync(task);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["Rooms"] = await _roomService.GetAllRoomsAsync();
            return View(task);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _housekeepingService.GetByIdAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CogStayMVC.Enums.TaskStatus taskStatus)
    {
        try
        {
            await _housekeepingService.UpdateTaskStatusAsync(id, taskStatus);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var dbTask = await _housekeepingService.GetByIdAsync(id);
            return View(dbTask);
        }
    }
}
