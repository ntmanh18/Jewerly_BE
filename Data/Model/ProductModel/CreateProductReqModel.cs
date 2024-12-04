using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.ProductModel
{
    public class CreateProductReqModel
    {
        public string ProductName { get; set; } = null!;

        public string Category { get; set; } = null!;

        public string Material { get; set; } = null!;

        public float Weight { get; set; }

        public long MachiningCost { get; set; }

        public float Size { get; set; }

        public int Amount { get; set; }

        public string? Desc { get; set; }

        public string Image { get; set; } = null!;
        public Dictionary<string, int> Gem { get; set; }
        public float MarkupRate { get; set; } = 1;

    }
}
