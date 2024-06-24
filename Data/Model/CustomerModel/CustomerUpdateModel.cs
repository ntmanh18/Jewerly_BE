using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.CustomerModel
{
    public class CustomerUpdateModel
    {
        public string CustomerId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public DateTime DoB { get; set; }

        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public int Point { get; set; }

        public string Rate { get; set; } = null!;
    }
}
