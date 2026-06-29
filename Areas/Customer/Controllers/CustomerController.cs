using Microsoft.AspNetCore.Mvc;

namespace CogStayMVC.Areas.Customer.Controllers;

[Area("Customer")]
public class CustomerController : Controller
{
    public IActionResult Login() => View();
    public IActionResult Dashboard() => View();
    public IActionResult AvailableRooms() => View();
    public IActionResult BookRoom() => View();
    public IActionResult BookingHistory() => View();
    public IActionResult Payment() => View();
    public IActionResult Profile() => View();
    public IActionResult Feedback() => View();
}
