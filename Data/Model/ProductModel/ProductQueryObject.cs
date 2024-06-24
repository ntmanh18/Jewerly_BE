using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.ProductModel
{
    public class ProductQueryObject
    {
        public string? ProductId { get; set; } = "";
        public string? ProductName { get; set; } = "";
        public string? Category { get; set; } = "";
        public string? Material { get; set; }= "";
    }
}
