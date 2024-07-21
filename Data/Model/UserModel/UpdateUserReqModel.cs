using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Data.Model.UserModel
{
   public class UpdateUserReqModel
    {
        public string? Username { get; set; } = null!;

        public string? FullName { get; set; } = null!;
        
        public DateTime DateOfBirth { get; set; }
        

        public string? Phone { get; set; } = null!;

        public string? Address { get; set; } = null!;

        

    }
}
