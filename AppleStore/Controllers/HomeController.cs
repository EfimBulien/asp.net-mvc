using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using AppleStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers
{
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
            var cart = new Cart();
            if (HttpContext.Session.Keys.Contains("Cart")) 
                cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart") ?? throw 
                    new InvalidOperationException());
            return View(cart);
        }

        public IActionResult AddToCart()
        {
            var id = Convert.ToInt32(Request.Query["ID"]);
            var cart = new Cart();
            if (HttpContext.Session.Keys.Contains("Cart")) 
                cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart") ?? throw 
                    new InvalidOperationException());
            cart?.CartLines.Add(context.Products.Find(id) ?? throw new InvalidOperationException());
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromCart()
        {
            var number = Convert.ToInt32(Request.Query["Number"]);
            var cart = new Cart();
            if (HttpContext.Session.Keys.Contains("Cart")) 
                cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart") ?? throw 
                    new InvalidOperationException());
            cart?.CartLines.RemoveAt(number); 
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            return RedirectToAction("Cart", "Home");
        }

        public IActionResult RemoveAllFromCart()
        {
            var id = Convert.ToInt32(Request.Query["ID"]);
            var cart = new Cart();
            if (HttpContext.Session.Keys.Contains("Cart")) 
                cart = JsonSerializer.Deserialize<Cart>(HttpContext.Session.GetString("Cart") ?? throw 
                    new InvalidOperationException());
            cart?.CartLines.RemoveAll(item => item.IDProduct == id); 
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            return RedirectToAction("Cart", "Home");
        }

        public IActionResult ManagerDashboard()
        {
            if (HttpContext.Session.GetInt32("RoleID") != 2) return RedirectToAction("SignIn", "Home");
            
            var totalSales = context.Orders.Count();
            
            var productSales = context.OrderProducts.GroupBy(op => op.Product.IDProduct)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSales = g.Sum(op => op.TotalCount),
                    ProductName = g.First().Product.ProductName
                }).ToList();

            var salesData = new
            {
                TotalSales = totalSales,
                ProductSales = productSales
            };

            return View(salesData);
        }
        
        public IActionResult AdminPanel()
        {
            if (HttpContext.Session.GetInt32("RoleID") != 1) 
                return RedirectToAction("SignIn", "Home");
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AuthUser ");
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            if (HttpContext.Session.Keys.Contains("AuthUser ")) 
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
                HttpContext.Session.SetString("AuthUser ", user.UserLogin);
                HttpContext.Session.SetInt32("RoleID", user.RoleID);
                await Authenticate(user.UserLogin);
                
                return user.RoleID switch
                {
                    1 => RedirectToAction("AdminPanel", "Home"),
                    2 => RedirectToAction("ManagerDashboard", "Home"),
                    3 => RedirectToAction("Index", "Home"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }

        private async Task Authenticate(string userLogin)
        {
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, userLogin)
            };
            
            var id = new ClaimsIdentity (
                claims, 
                "ApplicationCookie", 
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
            if (!ModelState.IsValid) 
                return View(user);
            
            if (user.UserPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Пароли не совпадают.");
                return View(user);
            }
            
            var existingUser  = await context.Users.FirstOrDefaultAsync(u => 
                u.UserLogin == user.UserLogin || u.UserEmail == user.UserEmail);

            if (existingUser  != null)
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
            throw new NotImplementedException();
        }
    }
} 