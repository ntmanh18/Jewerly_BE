using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Discount
{
    public string DiscountId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateOnly ExpiredDay { get; set; }

    public DateOnly PublishDay { get; set; }

    public long Cost { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Product> ProductProducts { get; set; } = new List<Product>();
}
