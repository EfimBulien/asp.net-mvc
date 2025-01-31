using System.Globalization;
using AppleStore.Data;
using AppleStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers;

public class CatalogController(ApplicationDbContext context) : Controller
{
    public IActionResult Catalog(string searchString, decimal? priceMin, decimal? priceMax, int? categoryId)
    {
        var products = context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(p => p.ProductName.ToLower().Contains(searchString.ToLower()));
        }

        if (priceMin.HasValue)
        {
            products = products.Where(p => p.ProductPrice >= priceMin.Value);
        }

        if (priceMax.HasValue)
        {
            products = products.Where(p => p.ProductPrice <= priceMax.Value);
        }

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryID == categoryId.Value);
        }

        ViewBag.Categories = context.Categories.ToList();
        ViewBag.CurrentFilter = searchString;
        ViewBag.PriceMin = priceMin.HasValue ? priceMin.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
        ViewBag.PriceMax = priceMax.HasValue ? priceMax.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
        ViewBag.CategoryId = categoryId.HasValue ? categoryId.Value.ToString() : string.Empty;

        return View(products.ToList());
    }

    public async Task<IActionResult> Product(int id)
    {
        var product = await context.Products
            .Include(p => p.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(p => p.IDProduct == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddReview(AddReviewViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Product", new { id = model.ProductId });
        }

        if (User.Identity != null)
        {
            var userId = User.Identity.Name;
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserLogin == userId);

            if (user == null)
            {
                return Unauthorized();
            }

            var review = new Review
            {
                ProductID = model.ProductId,
                UserID = user.IDUser,
                Rating = model.Rating,
                ReviewText = model.Text,
                CreatedAt = DateTime.Now
            };

            context.Reviews.Add(review);
        }

        await context.SaveChangesAsync();

        return RedirectToAction("Product", new { id = model.ProductId });
    }
}
