using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.BillModel
{
    public class BillSearchModel
    {
        public DateTime? stardate { get; set; } = null;
        public DateTime? enddate { get; set; } = null;
        public int? CashNumber { get; set; }
        public bool SortByTotalCost { get; set; }
        public bool SortByTotalCostDesc { get; set; }
    }
}
