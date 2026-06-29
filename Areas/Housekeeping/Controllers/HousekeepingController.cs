using Microsoft.AspNetCore.Mvc;

namespace CogStayMVC.Areas.Housekeeping.Controllers;

[Area("Housekeeping")]
public class HousekeepingController : Controller
{
    public IActionResult Login() => View();
    public IActionResult Dashboard() => View();
    public IActionResult ServiceRequests() => View();
    public IActionResult CleaningService() => View();
    public IActionResult LaundryService() => View();
    public IActionResult FoodService() => View();
    public IActionResult WellnessSpaRequest() => View();
    public IActionResult MaintenanceService() => View();
    public IActionResult AssignTask() => View();
}
