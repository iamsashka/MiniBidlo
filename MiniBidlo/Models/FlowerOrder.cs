using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class FlowerOrder
{
    public int IdOrder { get; set; }

    public int? IdUser { get; set; }

    public DateTime Date { get; set; }

    public decimal Sum { get; set; }

    public string? Status { get; set; }

    public string? ShippingAddress { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
