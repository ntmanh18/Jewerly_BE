using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Warranty
{
    public string WarrantyId { get; set; } = null!;

    public string CustomerCustomerId { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly ExpiredDate { get; set; }

    public string? Desc { get; set; }

    public string ProductId { get; set; } = null!;

    public virtual Customer CustomerCustomer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
