using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CogStayMVC.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("Guest"))
            {
                return RedirectToAction("Dashboard", "Guest");
            }
            return RedirectToAction("Dashboard", "Staff");
        }
        return View();
    }

    public IActionResult Error()
    {
        ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View();
    }
}
