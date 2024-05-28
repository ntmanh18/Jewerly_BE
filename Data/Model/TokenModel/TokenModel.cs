using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.TokenModel
{
    public class TokenModel
    {
        public string userid { get; set; }

        public string role{ get; set; }
        public TokenModel(string userid, string roleName)
        {
            this.userid = userid;
            this.role = roleName;
        }
    }
}
