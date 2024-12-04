using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Gem
{
    public string GemId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Type { get; set; }

    public long Price { get; set; }

    public string? Desc { get; set; }

    public float? Rate { get; set; }

    public virtual ICollection<ProductGem> ProductGems { get; set; } = new List<ProductGem>();
}
