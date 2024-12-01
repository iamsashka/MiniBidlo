using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<FlowerOrder> FlowerOrders { get; set; } = new List<FlowerOrder>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
