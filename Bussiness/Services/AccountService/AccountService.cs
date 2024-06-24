using Bussiness.Security;
using Bussiness.Services.AuthenticateService;
using Data.Model.AuthenticateModel;
using Data.Model.ResultModel;
using Data.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepo _userRepo;
        private readonly IAuthenticateService _authenticateService;

        public AccountService(IUserRepo userRepo, IAuthenticateService authenticateService)
        {
            _userRepo = userRepo;
            _authenticateService = authenticateService;
        }
        public async Task<ResultModel> LoginService(UserLoginReqModel user)
        {
            ResultModel res = new ResultModel();
            try
            {
                var existedUser = await _userRepo.GetByUsernameAsync(user.username);
                if (existedUser == null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Unauthorized;
                    res.Message = "User khong ton tai";
                    return res;
                }
                if (existedUser.Status.Equals(false))
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Ban khong co quyen truy cap";
                    return res;
                }
                bool isMatch = HashPass.VerifyPassword(user.password, existedUser.Password);

                if (isMatch == false)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Unauthorized;
                    res.Message = "Mat khau sai";
                    return res;
                }
                LoginResModel loginResModel = new LoginResModel()
                {
                    UserId = existedUser.UserId,
                    FullName = existedUser.FullName,
                    Role = existedUser.Role,
                    DoB = existedUser.DoB,
                    Phone = existedUser.Phone,
                    Address = existedUser.Address,
                    Status = existedUser.Status,
                };
                var token = _authenticateService.GenerateJWT(loginResModel);
                LoginTokenModel loginTokenModel = new LoginTokenModel()
                {
                    LoginResModel = loginResModel,
                    token = token
                };
                res.IsSuccess = true;
                res.Code = 200;
                res.Data = loginTokenModel;
                return res;
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Code = 400;
                res.Message = e.InnerException != null ? e.InnerException.Message + "\n" + e.StackTrace : e.Message + "\n" + e.StackTrace;
            }
            return res;
        }
        bool IAccountService.IsValidRole(string userRole, List<int> validRole)
        {
            return validRole.Any(role => role.ToString() == userRole);
        }
    }
}
