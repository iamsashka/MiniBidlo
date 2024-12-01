using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniBidlo.Models
{
    public partial class CartItem
    {
        public int IdCartItem { get; set; }
        public int? IdUser { get; set; }
        public int? IdProduct { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }

        // Навигационное свойство для связи с продуктом
        public virtual Product? IdProductNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }

        // Свойство для подсчета общей стоимости товара в корзине
        public decimal TotalPrice => (IdProductNavigation != null ? IdProductNavigation.Price : 0) * Quantity;

        // Метод для обновления количества товара
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new ArgumentException("Количество не может быть отрицательным");
            }

            Quantity = newQuantity;
        }

        // Метод для подсчета общей стоимости всех товаров в корзине
        public static decimal CalculateTotalCost(List<CartItem> cartItems)
        {
            return cartItems.Sum(item => item.TotalPrice);
        }
    }
}
