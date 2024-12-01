using Microsoft.AspNetCore.Mvc;
using MiniBidlo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniBidlo.Controllers
{
    public class CartController : Controller
    {
        private static List<CartItem> _cartItems = new List<CartItem>();
        private readonly FlowerMagazinContext _db;

        public CartController(FlowerMagazinContext context)
        {
            _db = context;
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItemModel model)
        {
            // Получаем продукт по ID
            var product = _db.Products.FirstOrDefault(p => p.IdProduct == model.ProductId);
            if (product == null)
            {
                return Json(new { success = false, message = "Продукт не найден." });
            }

            // Проверка, есть ли уже такой товар в корзине
            var existingItem = _cartItems.FirstOrDefault(x => x.IdProduct == model.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += model.Quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    IdProduct = model.ProductId,
                    Quantity = model.Quantity,
                    IdProductNavigation = product
                };
                _cartItems.Add(newItem);
            }

            return Json(new { success = true, message = "Товар добавлен в корзину." });
        }

        // Дополнительный метод для отображения корзины (необязательно)
        public IActionResult Index()
        {
            ViewBag.TotalCost = _cartItems.Sum(item => item.Quantity * item.IdProductNavigation.Price);
            return View(_cartItems);
        }
    }

    public class CartItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
