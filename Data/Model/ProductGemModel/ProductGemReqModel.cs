using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.ProductGemModel
{
    public class ProductGemReqModel
    {
        public string ProductId { get; set; }
        public Dictionary<string,int> Gem {  get; set; }
       
    }
}
