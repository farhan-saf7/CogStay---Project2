using Microsoft.AspNetCore.Mvc;

namespace CogStayMVC.Controllers;

public class StaffLoginController : Controller
{
    public IActionResult RoleSelect()
    {
        return View();
    }
}
