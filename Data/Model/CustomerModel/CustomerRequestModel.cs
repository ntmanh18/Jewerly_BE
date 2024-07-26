using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.CustomerModel
{
    public class CustomerRequestModel
    {
        public string FullName { get; set; } = null!;

        public DateOnly DoB { get; set; }

        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public int Point { get; set; }

        public string Rate { get; set; } = null!;
        public bool Status { get; set; }
    }
}
