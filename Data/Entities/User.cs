using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly DoB { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<Cashier> Cashiers { get; set; } = new List<Cashier>();

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<Gold> Golds { get; set; } = new List<Gold>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
