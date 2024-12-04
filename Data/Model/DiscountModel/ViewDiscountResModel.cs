using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.DiscountModel
{
    public class ViewDiscountResModel
    {
        public string DiscountId { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateOnly ExpiredDay { get; set; }

        public DateOnly PublishDay { get; set; }

        public int Amount { get; set; }

        public long Cost { get; set; }

    }
}
