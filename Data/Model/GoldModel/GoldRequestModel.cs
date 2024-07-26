using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.GoldModel
{
    public class GoldRequestModel
    {
        public string GoldId { get; set; } = null!;

        public string? GoldName { get; set; }

        public decimal? PurchasePrice { get; set; }

        public decimal? SalePrice { get; set; }

        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedDate { get; set; }

        public string? Kara { get; set; }

        public string? GoldPercent { get; set; }

        public decimal? WorldPrice { get; set; }
    }
}
