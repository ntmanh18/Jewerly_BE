using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Cashier
{
    public string CashId { get; set; } = null!;

    public DateTime StartCash { get; set; }

    public DateTime EndCash { get; set; }

    public long Income { get; set; }

    public int CashNumber { get; set; }

    public string UserId { get; set; } = null!;

    public int? Status { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual User User { get; set; } = null!;
}
