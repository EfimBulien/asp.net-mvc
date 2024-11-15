using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppleStore.Models;

namespace AppleStore.Controllers;

public class PayWaysController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> List()
    {
        var payWays = await context.PayWays.ToListAsync();
        return View(payWays);
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var payWay = await context.PayWays.FindAsync(id);
        if (payWay == null)
        {
            return NotFound();
        }
        return View(payWay);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PayWay payWay)
    {
        if (!ModelState.IsValid)
        {
            return View(payWay);
        }

        context.Update(payWay);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var payWay = await context.PayWays.FindAsync(id);
        if (payWay == null) return RedirectToAction("List");
        context.PayWays.Remove(payWay);
        await context.SaveChangesAsync();
        return RedirectToAction("List");
    }
}