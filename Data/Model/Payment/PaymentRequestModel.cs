using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.Payment
{
    public class PaymentRequestModel
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
