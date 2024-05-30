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
        
        public string? DateOfBirth { get; set; }
        [JsonIgnore]
        public DateTime? DoB
        {
            get { if (DateOfBirth.Length == 0) return null;
                else { return DateTime.Parse(DateOfBirth, new CultureInfo("en-US")); } }
           
        }

        public string? Phone { get; set; } = null!;

        public string? Address { get; set; } = null!;

        

    }
}
