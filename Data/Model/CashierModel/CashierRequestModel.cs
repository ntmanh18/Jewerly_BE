using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.CashierModel
{
    public class CashierRequestModel
    {

        public DateTime StartCash { get; set; }

        public DateTime EndCash { get; set; }

        public long Income { get; set; }

        public int CashNumber { get; set; }

        public string UserId { get; set; } = null!;

        public int? Status { get; set; }
    }
}
