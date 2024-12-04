using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.CustomerModel
{
    public class BillDto
    {
        public string BillId { get; set; } = null!;

        public decimal? TotalCost { get; set; }

        public DateTime PublishDay { get; set; }
        public string? CustomerId { get; set; }

    }
}
