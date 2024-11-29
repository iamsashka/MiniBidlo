using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class PosOrder
{
    public int IdPosOrder { get; set; }

    public int? IdOrder { get; set; }

    public int? IdProduct { get; set; }

    public int Count { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
