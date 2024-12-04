using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.DiscountModel
{
    public class CreateDiscountReqModel
    {
        public DateTime ExpiredDay { get; set; }

        public DateTime PublishDay { get; set; }

        public long Cost { get; set; }

    }
}
