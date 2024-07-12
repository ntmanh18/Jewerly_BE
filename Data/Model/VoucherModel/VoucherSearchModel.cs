using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.VoucherModel
{
    public class VoucherSearchModel
    {
        public DateTime? expiredDay { get; set; } = null;
        public string? customerId { get; set; } = null;
        public string? customerName { get; set; } = null;
        public string? customerPhone { get; set; } = null;
        public string? customerEmail { get; set; } = null;
    }
}
