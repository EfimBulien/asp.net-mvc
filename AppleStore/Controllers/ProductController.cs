using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppleStore.Models;

namespace AppleStore.Controllers;

public class ProductsController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> List()
    {
        var products = await context.Products.Include(p => p.Category).ToListAsync();
        return View(products);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = await context.Categories.ToListAsync();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await context.Categories.ToListAsync();
            return View(product);
        }

        context.Update(product);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return RedirectToAction("List");
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await context.Categories.ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await context.Categories.ToListAsync();
            return View(product);
        }

        context.Products.Add(product);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }
}
