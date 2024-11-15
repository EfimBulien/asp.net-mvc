using System.Diagnostics;
using System.Security.Claims;
using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using AppleStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public IActionResult Index()
    {
        var products = context.Products.Include(p => p.Category).ToList();
        return View(products);
    }
    
    public IActionResult Catalog()
    {
        var products = context.Products.Include(p => p.Category).ToList();
        return View(products);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Favorites()
    {
        return View();
    }

    public IActionResult Cart()
    {
        return View();
    }
    
    public IActionResult ManagerDashboard()
    {
        if (HttpContext.Session.GetInt32("RoleID") != 2) return RedirectToAction("SignIn", "Home"); 
        return View();
    }

    public IActionResult AdminPanel()
    {
        if (HttpContext.Session.GetInt32("RoleID") != 3) return RedirectToAction("SignIn", "Home");
        return View();
    }

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Remove("AuthUser");
        return RedirectToAction("SignIn");
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        if (HttpContext.Session.Keys.Contains("AuthUser")) return RedirectToAction("Index", "Home");
        return View(new LoginModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(LoginModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserLogin == model.Login && u.UserPassword == model.Password);
    
        if (user != null)
        {
            HttpContext.Session.SetString("AuthUser", user.UserLogin);
            HttpContext.Session.SetInt32("RoleID", user.RoleID);
        
            await Authenticate(user.UserLogin);
            Console.WriteLine(user.RoleID);
            if (user.Role != null)
            {
                switch (user.RoleID)
                {
                    case 1:
                        return RedirectToAction("AdminPanel", "Home");
                    case 2:
                        return RedirectToAction("ManagerDashboard", "Home");
                    case 3:
                        return RedirectToAction("Index", "Home");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }
        }
        
        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        return View(model);
    }

    private async Task Authenticate(string userLogin)
    {
        var claims = new List<Claim> { new(ClaimsIdentity.DefaultNameClaimType, userLogin) };
        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
    
    public IActionResult SignUp()
    {
        return View(new User());
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(User user, string confirmPassword)
    {
        if (user == null)
        {
            ModelState.AddModelError("", "Ошибка при создании пользователя.");
            return View(new User());
        }
        
        if (!ModelState.IsValid) return View(user);
        
        if (user.UserPassword != confirmPassword)
        {
            ModelState.AddModelError("", "Пароли не совпадают.");
            return View(user);
        }
        
        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.UserLogin == user.UserLogin || u.UserEmail == user.UserEmail);

        if (existingUser != null)
        {
            ModelState.AddModelError("", "Логин или email уже заняты.");
            return View(user);
        }
        
        user.CreationDate = DateTime.Now;
        user.RoleID = 3;
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return RedirectToAction("SignIn");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}