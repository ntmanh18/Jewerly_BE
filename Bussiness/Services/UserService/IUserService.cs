using Data.Model.ResultModel;
using Data.Model.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.UserService
{
    public interface IUserService
    {
        Task<ResultModel> CreateUser(string token, CreateUserReqModel model);
        Task<ResultModel> UpdateUser(string token, UpdateUserReqModel model);
        Task<ResultModel> DeactiveUser(string token, DeactiveUserReqModel model);
        Task<ResultModel> UpdateRole(string token, UpdateRoleReqModel model);
        Task<ResultModel> ViewUserList(string token, UserQueryObject query);
        Task<ResultModel> ChangePassword(string token, ChangePasswordModel model);
        

    }
}
