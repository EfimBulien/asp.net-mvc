using Microsoft.AspNetCore.Mvc;
using AppleStore.Models;
using AppleStore.Data;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.Controllers
{
    public class OrdersController(ApplicationDbContext context) : Controller
    {
        public async Task<IActionResult> List()
        {
            var orders = await context.Orders
                .Include(o => o.User)
                .Include(o => o.PayWay)
                .ToListAsync();
            return View(orders);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Order());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (!ModelState.IsValid) return View(order);
            context.Orders.Add(order);
            context.SaveChanges();
            return RedirectToAction("List");
        }
        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var order = context.Orders.Find(id);
            if (order == null) return NotFound();
            return View(order);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Order order)
        {
            if (!ModelState.IsValid) return View(order);
            context.Orders.Update(order);
            context.SaveChanges();
            return RedirectToAction("List");
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var order = await context.Orders.FindAsync(id);
            if (order == null) return RedirectToAction("List");
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}