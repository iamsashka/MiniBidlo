using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class User
{
    public int IdUser { get; set; }
    public string Login { get; set; }
    public string Password { get; set; } // Сюда будет храниться хэшированный пароль
    public string Email { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }

    public virtual ICollection<FlowerOrder> FlowerOrders { get; set; } = new List<FlowerOrder>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
