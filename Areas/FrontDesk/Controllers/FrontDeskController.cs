using Microsoft.AspNetCore.Mvc;

namespace CogStayMVC.Areas.FrontDesk.Controllers;

[Area("FrontDesk")]
public class FrontDeskController : Controller
{
    public IActionResult Login() => View();
    public IActionResult Dashboard() => View();
    public IActionResult CheckIn() => View();
    public IActionResult CheckOut() => View();
    public IActionResult Reservations() => View();
    public IActionResult HousekeepingCoordination() => View();
    public IActionResult GuestOperations() => View();
}
