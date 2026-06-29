using Microsoft.AspNetCore.Mvc;

namespace CogStayMVC.Areas.Manager.Controllers;

[Area("Manager")]
public class ManagerController : Controller
{
    public IActionResult Login() => View();
    public IActionResult Dashboard() => View();
    public IActionResult Reservations() => View();
    public IActionResult Occupancy() => View();
    public IActionResult Revenue() => View();
    public IActionResult HousekeepingOverview() => View();
    public IActionResult Feedbacks() => View();
}
