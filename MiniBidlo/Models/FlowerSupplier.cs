using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class FlowerSupplier
{
    public int IdSupplier { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Product> IdProducts { get; set; } = new List<Product>();
}
