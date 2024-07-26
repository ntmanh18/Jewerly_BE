using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class DiscountProduct
{
    public string DiscountDiscountId { get; set; } = null!;

    public string ProductProductId { get; set; } = null!;

    public string? Desc { get; set; }

    public virtual Discount DiscountDiscount { get; set; } = null!;

    public virtual Product ProductProduct { get; set; } = null!;
}
