using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.VoucherModel
{
    public class VoucherRequestModel
    {
        public string VoucherId { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;

        public DateTime ExpiredDay { get; set; }

        public long Cost { get; set; }

        public string CustomerCustomerId { get; set; } = null!;
    }
}
