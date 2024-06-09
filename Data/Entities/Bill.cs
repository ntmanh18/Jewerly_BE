using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Bill
{
    public string BillId { get; set; } = null!;

    public long TotalCost { get; set; }

    public DateTime PublishDay { get; set; }

    public string? VoucherVoucherId { get; set; }

    public string CashierId { get; set; } = null!;

    public string? CustomerId { get; set; }

    public virtual Cashier Cashier { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OldProduct> OldProducts { get; set; } = new List<OldProduct>();

    public virtual ICollection<ProductBill> ProductBills { get; set; } = new List<ProductBill>();

    public virtual Voucher? VoucherVoucher { get; set; }
}
