using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.GoldModel
{
    public class GoldCreateModel
    {
        public string? GoldName { get; set; }

        public long PurchasePrice { get; set; }

        public long SalePrice { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string? Kara { get; set; }

        public string? GoldPercent { get; set; }

        public float? WorldPrice { get; set; }
    }
}
