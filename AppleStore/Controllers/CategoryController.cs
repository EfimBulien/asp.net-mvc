using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppleStore.Models;

namespace AppleStore.Controllers;

public class CategoriesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> List()
    {
        var categories = await context.Categories.ToListAsync();
        return View(categories);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var category = context.Categories.Find(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category category)
    {
        if (!ModelState.IsValid) return View(category);
        context.Categories.Update(category);
        context.SaveChanges();
        return RedirectToAction("List");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null) return RedirectToAction("List");
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (!ModelState.IsValid) return View(category);
        context.Categories.Add(category);
        context.SaveChanges();
        return RedirectToAction("List");
    }
}