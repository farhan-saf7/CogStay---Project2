using Microsoft.AspNetCore.Mvc;

namespace CogStayMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminController : Controller
{
    public IActionResult Login() => View();
    public IActionResult Dashboard() => View();
    public IActionResult RoomManagement() => View();
    public IActionResult ReservationMonitoring() => View();
    public IActionResult Reports() => View();
    public IActionResult StaffUserManagement() => View();
    public IActionResult Settings() => View();
}
