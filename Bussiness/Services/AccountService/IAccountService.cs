using Data.Model.AuthenticateModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.AccountService
{
    public interface IAccountService
    {
        public Task<ResultModel> LoginService(UserLoginReqModel user);
    }
}
