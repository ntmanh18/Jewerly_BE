using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.WarrantyModel
{
    public class WarrantySearchModel
    {
        public string? warrantyId { get; set; } = null;
        public string? productId { get; set; } = null;
        public string? productName { get; set;} = null;
        public string? customerId { get; set;} = null;
        public string? customerName { get; set;} = null;
    }
}
