using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly DoB { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Point { get; set; }

    public string Rate { get; set; } = null!;

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();

    public virtual ICollection<Warranty> Warranties { get; set; } = new List<Warranty>();
}
