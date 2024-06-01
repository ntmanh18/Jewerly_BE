using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.UserModel
{
    public class ViewUserResModel
    {
        public string UserId { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int Role { get; set; }

        public string FullName { get; set; } = null!;

        public DateOnly DoB { get; set; }

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        public bool Status { get; set; }
    }
}
