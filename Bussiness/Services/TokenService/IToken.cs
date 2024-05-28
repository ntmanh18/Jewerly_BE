using Data.Model.TokenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.TokenService
{
    public interface IToken
    {
        TokenModel decode(string token);
    }
}
