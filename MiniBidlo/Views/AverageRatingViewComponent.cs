using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBidlo.Models;

public class AverageRatingViewComponent : ViewComponent
{
    private readonly FlowerMagazinContext _context;

    public AverageRatingViewComponent(FlowerMagazinContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int productId)
    {
        var averageRating = await _context.Reviews
            .Where(r => r.IdProduct == productId)
            .AverageAsync(r => (double?)r.Rating) ?? 0;

        return View(averageRating);
    }
}