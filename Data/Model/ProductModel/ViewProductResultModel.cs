using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.ProductModel
{
    public class ViewProductResultModel
    {
        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string Material { get; set; } = null!;

        public float Weight { get; set; }

        public long MachiningCost { get; set; }

        public float Size { get; set; }

        public int Amount { get; set; }

        public string? Desc { get; set; }

        public string Image { get; set; } = null!;
        public Dictionary<string, int> ProductGems { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal PriceWithDiscount { get; set; } = 0;
        public List<Discount> Discount { get; set; } = new List<Discount>();

    }
}
