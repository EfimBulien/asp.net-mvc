using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppleStore.Models;

namespace AppleStore.Controllers;

public class RolesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> List()
    {
        var roles = await context.Roles.ToListAsync();
        return View(roles);
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var role = await context.Roles.FindAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Role role)
    {
        if (!ModelState.IsValid)
        {
            return View(role);
        }

        context.Update(role);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var role = await context.Roles.FindAsync(id);
        if (role == null) return RedirectToAction("List");
        context.Roles.Remove(role);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }
}