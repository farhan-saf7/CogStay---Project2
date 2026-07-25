using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Handles base landing page navigation, showing available room summaries to public users,
/// and handles global application error tracking.
/// </summary>
public class HomeController : Controller
{
    private readonly IRoomService _roomService;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="roomService">Service for managing room queries and details.</param>
    public HomeController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    /// <summary>
    /// Renders the public-facing homepage displaying room selections.
    /// </summary>
    /// <returns>Index View displaying available rooms.</returns>
    public async Task<IActionResult> Index()
    {
        var availableRooms = await _roomService.GetAvailableRoomsAsync();
        return View(availableRooms);
    }

    /// <summary>
    /// Captures request IDs and displays application error diagnostics.
    /// </summary>
    /// <returns>Error View containing request trace details.</returns>
    public IActionResult Error()
    {
        ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View();
    }
}
