using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class Supplier
{
    public int IdSupplier { get; set; }

    public string Name { get; set; } = null!;

    public string? PhoneNumber { get; set; }
}
