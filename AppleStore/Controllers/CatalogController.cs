using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers;

public class CatalogController(ApplicationDbContext context) : Controller
{
    public IActionResult Catalog(string searchString, decimal? priceMin, decimal? priceMax)
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
        return View(products.ToList());
    }
}