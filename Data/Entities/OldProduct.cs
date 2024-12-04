using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class OldProduct
{
    public string OproductId { get; set; } = null!;

    public string ProductProductId { get; set; } = null!;

    public string? Desc { get; set; }

    public string BillBillId { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Bill BillBill { get; set; } = null!;

    public virtual Product ProductProduct { get; set; } = null!;
}
