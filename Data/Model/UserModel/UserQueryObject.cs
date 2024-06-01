using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.UserModel
{
   public class UserQueryObject
    {
        public int role { get; set; } = 0;
        public string? id { get; set; } = null;
        public string? name { get; set; } = null;
        public bool? status { get; set; }

        public string? sortBy { get; set; } = null;
        public bool isDescending { get; set; } = true;
    }
}
