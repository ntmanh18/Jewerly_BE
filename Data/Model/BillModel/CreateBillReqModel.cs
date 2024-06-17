using Data.Model.ProductBillModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.BillModel
{
    public class CreateBillReqModel { 
        public Dictionary<string, int> Product { get; set; }
        public string VoucherId { get; set; }
        public string? CustomerId { get; set; }
    }
}
