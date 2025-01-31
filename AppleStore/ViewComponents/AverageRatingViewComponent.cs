using AppleStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppleStore.ViewComponents;

public class AverageRatingViewComponent(ApplicationDbContext context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int productId)
    {
        var reviews = await context.Reviews
            .Where(r => r.ProductID == productId)
            .ToListAsync();

        var averageRating = reviews.Any() 
            ? reviews.Average(r => r.Rating) 
            : 0;

        return View(averageRating);
    }
}