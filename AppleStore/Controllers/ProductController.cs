/*
using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers;

public class ProductController(ApplicationDbContext context) : Controller
{
    public IActionResult Index()
    {
        var products = context.Products.Include(p => p.Category).ToList();
        return View(products);
    }
    
    public IActionResult Details(int id)
    {
        var product = context.Products.FirstOrDefault(p => p.IDProduct == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
}
*/