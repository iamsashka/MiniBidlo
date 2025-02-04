﻿using System;
using System.Collections.Generic;

namespace MiniBidlo.Models;

public partial class Review
{
    public int IdReview { get; set; }

    public int? IdUser { get; set; }

    public int? IdProduct { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product? IdProductNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }

}
