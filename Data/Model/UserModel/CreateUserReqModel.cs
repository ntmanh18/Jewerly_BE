using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.UserModel
{
    public class CreateUserReqModel
    {
        public int Role { get; set; }

        public string FullName { get; set; } = null!;

        public DateTime DoB { get; set; }

        public string Phone { get; set; } = null!;

        public string Address { get; set; } = null!;

        
    }
}
