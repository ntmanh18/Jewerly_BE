using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Voucher
{
    public string VoucherId { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateOnly ExpiredDay { get; set; }

    public DateOnly PublishedDay { get; set; }

    public long Cost { get; set; }

    public string CustomerCustomerId { get; set; } = null!;

    public bool? Status { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer CustomerCustomer { get; set; } = null!;
}
