using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? CategoryId { get; set; }

    public int StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<PosOrder> PosOrders { get; set; } = new List<PosOrder>();

    public virtual ICollection<FlowerSupplier> IdSuppliers { get; set; } = new List<FlowerSupplier>();
}
