using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class ProductWarrantty
{
    public string WarrantyId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public string? Desc { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warranty Warranty { get; set; } = null!;
}
