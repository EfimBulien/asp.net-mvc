using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using AppleStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    private const string CartCookieName = "UserCart";
    private const string FavoritesCookieName = "UserFavorites";
    private const string AuthSessionName = "AuthUser";
    internal const string RoleSessionName = "RoleID";
    private const string CartSessionName = "Cart";
    
    public IActionResult Index()
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
        var favorites = GetFavoritesFromCookies();
        var favoriteProducts = context.Products
            .Include(p => p.Category)
            .Where(p => favorites.Contains(p.IDProduct))
            .ToList();
        return View(favoriteProducts);
    }

    public IActionResult Cart()
    {
        var cart = GetCartFromCookies();
        return View(cart);
    }

    public IActionResult AddToCart(int id)
    {
        var cart = GetCartFromCookies();
        var product = context.Products.Find(id);
        if (product == null) 
            return RedirectToAction("Index", "Home");
        cart.CartLines.Add(product);
        SaveCartToCookies(cart);
        SaveCartToSession(cart);
        return RedirectToAction("Index", "Home");
    }
    
    private void SaveCartToSession(Cart cart)
    {
        HttpContext.Session.SetString(CartSessionName, JsonSerializer.Serialize(cart));
    }

    public IActionResult RemoveFromCart(int number)
    {
        var cart = GetCartFromCookies();
        if (number < 0 || number >= cart.CartLines.Count) return RedirectToAction("Cart", "Home");

        cart.CartLines.RemoveAt(number);
        SaveCartToCookies(cart);

        return RedirectToAction("Cart", "Home");
    }

    public IActionResult RemoveAllFromCart()
    {
        SaveCartToCookies(new Cart());
        return RedirectToAction("Cart", "Home");
    }

    private Cart GetCartFromCookies()
    {
        var cart = new Cart();
        var userLogin = HttpContext.Session.GetString(AuthSessionName);

        if (string.IsNullOrEmpty(userLogin)) return cart;

        var cookieValue = Request.Cookies[$"{CartCookieName}_{userLogin}"];
        if (!string.IsNullOrEmpty(cookieValue))
        {
            cart = JsonSerializer.Deserialize<Cart>(cookieValue) ?? new Cart();
        }

        return cart;
    }

    private void SaveCartToCookies(Cart cart)
    {
        var userLogin = HttpContext.Session.GetString(AuthSessionName);
        if (string.IsNullOrEmpty(userLogin)) return;

        var cartJson = JsonSerializer.Serialize(cart);
        Response.Cookies.Append($"{CartCookieName}_{userLogin}", cartJson, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(7),
            HttpOnly = true
        });
    }
    
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Remove(AuthSessionName);
        HttpContext.Session.Remove(CartSessionName); 
        return RedirectToAction("SignIn");
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        if (HttpContext.Session.Keys.Contains(AuthSessionName))
            return RedirectToAction("Index", "Home");
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
            HttpContext.Session.SetString(AuthSessionName, user.UserLogin);
            HttpContext.Session.SetInt32(RoleSessionName, user.RoleID);
            await Authenticate(user.UserLogin);

            return user.RoleID switch
            {
                1 => RedirectToAction("Admin", "Admin"),
                2 => RedirectToAction("Manager", "Manager"),
                3 => RedirectToAction("Index", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }
        
        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        return View(model);
    }

    private async Task Authenticate(string userLogin)
    {
        var claims = new List<Claim> { new(ClaimsIdentity.DefaultNameClaimType, userLogin) };

        var id = new ClaimsIdentity(
            claims, "ApplicationCookie", 
            ClaimsIdentity.DefaultNameClaimType,
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
        if (!ModelState.IsValid) return View(user);
        if (user.UserPassword != confirmPassword)
        {
            ModelState.AddModelError("", "Пароли не совпадают.");
            return View(user);
        }

        var existingUser = await context.Users.FirstOrDefaultAsync(u =>
            u.UserLogin == user.UserLogin || u.UserEmail == user.UserEmail);

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
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }

    public IActionResult Checkout()
    {
        var cart = GetCartFromCookies();
        if (cart.CartLines.Count == 0)
        {
            TempData["Error"] = "Корзина пуста.";
            return RedirectToAction("Cart");
        }
        
        var checkoutDetails = GenerateCheckoutDetails(cart);
        var filePath = SaveCheckoutDetailsToFile(checkoutDetails);

        if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
        {
            TempData["Error"] = "Не удалось сохранить чек.";
            return RedirectToAction("Cart");
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        SaveCartToSession(new Cart());

        return File(fileBytes, "text/plain", "checkout.txt");
    }
    
    private static string GenerateCheckoutDetails(Cart cart)
    {
        var checkoutDetails = new System.Text.StringBuilder();
        checkoutDetails.AppendLine("Checkout Details:");
        checkoutDetails.AppendLine("-----------------");

        foreach (var item in cart.CartLines)
        {
            checkoutDetails.AppendLine($"Product: {item.ProductName}, Price: {item.ProductPrice}, Quantity: 1");
        }

        checkoutDetails.AppendLine("-----------------");
        checkoutDetails.AppendLine($"Total Price: {cart.FinalPrice}");
        return checkoutDetails.ToString();
    }

    private static string SaveCheckoutDetailsToFile(string checkoutDetails)
    {
        try
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Checkouts");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"checkout_{DateTime.Now:yyyyMMddHHmmss}.txt";
            var filePath = Path.Combine(directoryPath, fileName);
            System.IO.File.WriteAllText(filePath, checkoutDetails);
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении чека: {ex.Message}");
            return string.Empty;
        }
    }
        
    public IActionResult AddToFavorites(int id)
    {
        var favorites = GetFavoritesFromCookies();
        if (favorites.Contains(id)) return RedirectToAction("Favorites");

        favorites.Add(id);
        SaveFavoritesToCookies(favorites);
        return RedirectToAction("Favorites");
    }

    public IActionResult RemoveFromFavorites(int id)
    {
        var favorites = GetFavoritesFromCookies();
        if (!favorites.Contains(id)) return RedirectToAction("Favorites");

        favorites.Remove(id);
        SaveFavoritesToCookies(favorites);
        return RedirectToAction("Favorites");
    }

    private List<int> GetFavoritesFromCookies()
    {
        var favorites = new List<int>();
        var userLogin = HttpContext.Session.GetString(AuthSessionName);

        if (string.IsNullOrEmpty(userLogin)) return favorites;

        var cookieValue = Request.Cookies[$"{FavoritesCookieName}_{userLogin}"];
        if (!string.IsNullOrEmpty(cookieValue))
        {
            favorites = JsonSerializer.Deserialize<List<int>>(cookieValue) ?? new List<int>();
        }
        return favorites;
    }

    private void SaveFavoritesToCookies(List<int> favorites)
    {
        var userLogin = HttpContext.Session.GetString(AuthSessionName);
        if (string.IsNullOrEmpty(userLogin)) return;

        var favoritesJson = JsonSerializer.Serialize(favorites);
        Response.Cookies.Append($"{FavoritesCookieName}_{userLogin}", favoritesJson, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(7),
            HttpOnly = true
        });
    }

}
