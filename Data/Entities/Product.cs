using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Material { get; set; } = null!;

    public float Weight { get; set; }

    public long MachiningCost { get; set; }

    public float Size { get; set; }

    public int Amount { get; set; }

    public string? Desc { get; set; }

    public string Image { get; set; } = null!;

    public float? MarkupRate { get; set; }

    public virtual Gold MaterialNavigation { get; set; } = null!;

    public virtual ICollection<OldProduct> OldProducts { get; set; } = new List<OldProduct>();

    public virtual ICollection<ProductBill> ProductBills { get; set; } = new List<ProductBill>();

    public virtual ICollection<ProductGem> ProductGems { get; set; } = new List<ProductGem>();

    public virtual ICollection<Warranty> Warranties { get; set; } = new List<Warranty>();

    public virtual ICollection<Discount> DiscountDiscounts { get; set; } = new List<Discount>();
}
