using Microsoft.AspNetCore.Mvc;
using MiniBidlo.Models;
using System.Collections.Generic;
using System.Linq;

namespace MiniBidlo.Controllers
{
    public class CartController : Controller
    {
        // Предположим, что у нас есть хранилище данных для корзины (например, в виде базы данных или кэш)
        private readonly List<CartItem> _cartItems = new List<CartItem>();

        // Метод для отображения корзины с подсчетом общей стоимости
        public IActionResult Index()
        {
            decimal totalCost = CartItem.CalculateTotalCost(_cartItems);
            ViewBag.TotalCost = totalCost;

            return View(_cartItems);
        }

        // Метод для обновления количества товара
        [HttpPost]
        public IActionResult UpdateQuantity(int idCartItem, int newQuantity)
        {
            var item = _cartItems.FirstOrDefault(x => x.IdCartItem == idCartItem);

            if (item != null)
            {
                // Обновляем количество товара в корзине
                item.UpdateQuantity(newQuantity);

                // Возвращаем успешный результат
                return Json(new { success = true, message = "Количество товара обновлено." });
            }
            else
            {
                return Json(new { success = false, message = "Товар не найден в корзине." });
            }
        }

        // Метод для подсчета общей стоимости товаров в корзине
        public IActionResult GetTotalCost()
        {
            decimal totalCost = CartItem.CalculateTotalCost(_cartItems);
            return Json(new { totalCost = totalCost });
        }
    }
}
