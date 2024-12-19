using Microsoft.AspNetCore.Mvc;

namespace AppleStore.Controllers;

public class AdminController : Controller
{
    public IActionResult Admin()
    {
        if (HttpContext.Session.GetInt32(HomeController.RoleSessionName) != 1) 
            return RedirectToAction("SignIn", "Home");
        return View();
    }
}