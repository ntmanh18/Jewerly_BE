using Data.Model.VoucherModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.WarrantyModel
{
    public class WarrantyUpdateModel
    {
        public string WarrantyId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public string Desc { get; set; }

        public string ProductId { get; set; } = null!;
    }
}
