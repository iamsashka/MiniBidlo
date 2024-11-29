using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBidlo.Models;

namespace MiniBidlo.Controllers
{
    public class CatalogController : Controller

    {
        public FlowerMagazinContext db;

        public CatalogController(FlowerMagazinContext context)
        {
            db = context;
        }
        // GET: CatalogController
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id != null)
            { 
                Product product = await db.Products.FirstOrDefaultAsync(p => p.IdProduct == id);
            if (product != null)
                return View(product);

        }
        return NotFound(); 
    }


    }
}
