﻿using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class CartItem
{
    public int IdCartItem { get; set; }

    public int? IdUser { get; set; }

    public int? IdProduct { get; set; }

    public int Quantity { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Product? IdProductNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
    public decimal TotalPrice => IdProductNavigation?.Price * Quantity ?? 0;
}
