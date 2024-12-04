using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.ProductBillModel
{
    public class CreateProductBillReqModel
    {
        public string BillId { get; set; }
        public Dictionary<string,int> Product {  get; set; }
    }
}
