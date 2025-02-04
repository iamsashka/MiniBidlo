using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using MiniBidlo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class CatalogController : Controller
{
    private readonly FlowerMagazinContext _context;

    public CatalogController(FlowerMagazinContext context)
    {
        _context = context;
    }

    // Просмотр каталога
    public async Task<ActionResult> Index()
    {
        // Проверяем, авторизован ли пользователь
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToAction("Index", "Autorization");  // Перенаправляем на страницу авторизации
        }

        var products = await _context.Products.Include(p => p.Category).ToListAsync();
        var categories = await _context.Categories.ToListAsync(); // Получаем все категории

        // Передаем продукты и категории в представление
        ViewBag.Categories = new SelectList(categories, "IdCategory", "CategoryName");

        return View(products);
    }

  
    public async Task<IActionResult> ProductDetail(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
        .Include(p => p.Category)
        .Include(p => p.Reviews) // Загружаем отзывы
            .ThenInclude(r => r.IdUserNavigation) // Загружаем пользователя для каждого отзыва
        .FirstOrDefaultAsync(p => p.IdProduct == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    [HttpPost]
    
    public IActionResult AddReview(Review review)
    {
        if (ModelState.IsValid)
        {
            // Получаем ID авторизованного пользователя
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Autorization"); // Перенаправляем на страницу авторизации, если пользователь не авторизован
            }

            // Заполняем недостающие данные
            review.IdUser = userId.Value;
            review.CreatedAt = DateTime.Now;

            // Добавляем отзыв в базу данных
            _context.Reviews.Add(review);
            _context.SaveChanges();

            // Перенаправляем на страницу товара
            return RedirectToAction("ProductDetail", new { id = review.IdProduct });
        }

        // Если данные невалидны, возвращаем обратно на страницу товара
        return RedirectToAction("ProductDetail", new { id = review.IdProduct });
    }

    // Метод добавления товара в корзину
    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        // Проверка авторизации
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return Json(new { success = false, message = "Пожалуйста, войдите в систему." });
        }

        var product = _context.Products.FirstOrDefault(p => p.IdProduct == productId);

        if (product == null)
        {
            return Json(new { success = false, message = "Товар не найден." });
        }

        if (product.StockQuantity < quantity)
        {
            return Json(new { success = false, message = $"Недостаточно товара на складе. Доступное количество: {product.StockQuantity}." });
        }

        var cartItem = _context.CartItems.FirstOrDefault(ci => ci.IdProduct == productId && ci.IdUser == userId.Value);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new CartItem
            {
                IdUser = userId.Value,
                IdProduct = productId,
                Quantity = quantity,
                AddedAt = DateTime.Now
            };
            _context.CartItems.Add(cartItem);
        }

        _context.SaveChanges();

        return Json(new { success = true, message = "Товар добавлен в корзину." });
    }
}
