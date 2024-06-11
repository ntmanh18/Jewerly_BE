using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.VoucherModel
{
    public class VoucherCreateModel
    {
        public string CreatedBy { get; set; } = null!;

        public DatePart ExpiredDay { get; set; }
        public DatePart PublishedDay { get; set; }

        public long Cost { get; set; }

        public string CustomerCustomerId { get; set; } = null!;
    }
}
