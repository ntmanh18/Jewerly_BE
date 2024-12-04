using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class ProductBill
{
    public string ProductProductId { get; set; } = null!;

    public string BillBillId { get; set; } = null!;

    public int Amount { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Bill BillBill { get; set; } = null!;

    public virtual Product ProductProduct { get; set; } = null!;
}
