using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppleStore.Models;

namespace AppleStore.Controllers;

public class UsersController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> List()
    {
        var users = await context.Users.Include(u => u.Role).ToListAsync();
        return View(users);
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        ViewBag.Roles = await context.Roles.ToListAsync();
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(User user)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await context.Roles.ToListAsync();
            return View(user);
        }

        context.Update(user);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return RedirectToAction("List");
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }
}