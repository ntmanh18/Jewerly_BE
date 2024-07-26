using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.DiscountModel
{
    public class DiscountQueryModel
    {
        public string? id { get; set; } = null;
        public bool? status {  get; set; } = true;
        public string? productId { get; set; } = null;

    }
}
