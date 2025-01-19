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
            // Проверка на корректность количества товара
            if (model.Quantity <= 0)
            {
                return Json(new { success = false, message = "Количество товара должно быть больше нуля." });
            }

            var product = _db.Products.FirstOrDefault(p => p.IdProduct == model.ProductId);
            if (product == null)
            {
                return Json(new { success = false, message = "Продукт не найден." });
            }

            // Получаем остаток товара на складе
            int availableQuantity = product.StockQuantity;

            // Проверяем, не превышает ли добавляемое количество доступное на складе
            if (model.Quantity > availableQuantity)
            {
                return Json(new { success = false, message = $"Недостаточно товара на складе. Доступно только {availableQuantity} единиц." });
            }

            var existingItem = _cartItems.FirstOrDefault(x => x.IdProduct == model.ProductId);
            if (existingItem != null)
            {
                // Проверяем, не превысит ли количество товара в корзине доступное количество на складе
                if (existingItem.Quantity + model.Quantity > availableQuantity)
                {
                    return Json(new { success = false, message = $"Недостаточно товара на складе. Доступно только {availableQuantity - existingItem.Quantity} единиц." });
                }

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


        public IActionResult Index()
        {
            ViewBag.TotalCost = _cartItems.Sum(item => item.Quantity * item.IdProductNavigation.Price);
            return View(_cartItems);
        }
        // Метод для обновления количества товара в корзине
        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] UpdateQuantityModel model)
        {
            var item = _cartItems.FirstOrDefault(x => x.IdCartItem == model.IdCartItem);
            if (item == null)
            {
                return Json(new { success = false, message = "Товар не найден в корзине." });
            }

            var product = _db.Products.FirstOrDefault(p => p.IdProduct == item.IdProduct);
            if (product == null)
            {
                return Json(new { success = false, message = "Продукт не найден." });
            }

            // Получаем остаток товара на складе
            int availableQuantity = product.StockQuantity;

            // Проверяем, не превышает ли новое количество доступное на складе
            if (model.NewQuantity > availableQuantity)
            {
                return Json(new { success = false, message = $"Недостаточно товара на складе. Доступно только {availableQuantity} единиц." });
            }

            // Обновляем количество товара в корзине
            item.Quantity = model.NewQuantity;

            // Пересчитываем общую стоимость
            var totalCost = _cartItems.Sum(i => i.Quantity * i.IdProductNavigation.Price);

            return Json(new { success = true, message = "Количество товара обновлено.", totalCost });
        }


        // Метод для удаления товара из корзины
        [HttpPost]
        public IActionResult RemoveItem([FromBody] RemoveItemModel model)
        {
            var item = _cartItems.FirstOrDefault(x => x.IdCartItem == model.IdCartItem);
            if (item != null)
            {
                _cartItems.Remove(item);
            }

            // Пересчитываем общую стоимость
            var totalCost = _cartItems.Sum(i => i.Quantity * i.IdProductNavigation.Price);

            return Json(new { success = true, message = "Товар удален.", totalCost });
        }

        // Модели для обновления и удаления
        public class UpdateQuantityModel
        {
            public int IdCartItem { get; set; }
            public int NewQuantity { get; set; }
        }

        public class RemoveItemModel
        {
            public int IdCartItem { get; set; }
        }

    }

    public class CartItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
