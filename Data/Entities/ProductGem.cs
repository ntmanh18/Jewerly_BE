using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class ProductGem
{
    public string ProductProductId { get; set; } = null!;

    public string GemGemId { get; set; } = null!;

    public int? Amount { get; set; }

    public virtual Gem GemGem { get; set; } = null!;

    public virtual Product ProductProduct { get; set; } = null!;
}
