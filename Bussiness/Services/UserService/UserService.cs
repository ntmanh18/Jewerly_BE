using Bussiness.Security;
using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Bussiness.Services.Validate;
using Data.Entities;
using Data.Model.ResultModel;
using Data.Model.UserModel;
using Data.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IUserValidate _userValidate;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;
        public UserService(IUserRepo userRepo,
            IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService,
            IUserValidate userValidate
            )
        {
            _userRepo = userRepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
            _userValidate = userValidate;

        }
        public async Task<ResultModel> CreateUser(string token, CreateUserReqModel model)
        {
            var res = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2, 3 });
            if (!isValidRole)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";

                return res;
            }
            var isPhoneValid = await _userValidate.IsPhoneValid(model.Phone);
            if (isPhoneValid != null) { return isPhoneValid; }


            string id = await GenerateID();
            string username = GenerateUsername(model.FullName, id);
            string password = username;
            string hashedPass = HashPass.HashPassword(password);
            User user = new User()
            {
                UserId = id,
                Username = username,
                Password = hashedPass,
                FullName = model.FullName,
                Role = model.Role,
                DoB = DateOnly.FromDateTime(model.DoB),
                Phone = model.Phone,
                Address = model.Address,
                Status = true,
            };

            await _userRepo.Insert(user);
            res.IsSuccess = true;
            res.Code = (int)HttpStatusCode.OK;
            res.Message = "Create user successfully";
            res.Data = password;
            return res;


        }

        public async Task<ResultModel> UpdateUser(string token, UpdateUserReqModel model)
        {
            var res = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2, 3 });
            if (!isValidRole)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";

                return res;
            }
            var existingUser =await _userRepo.GetByIdAsync(decodeModel.userid);
            if (existingUser == null)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Data = null,
                    Message = "User doesn't exist"
                };
            }
            var isPhoneValid = await _userValidate.IsPhoneValid(model.Phone);
            
            if(model.Username.Length > 0)
            {
                existingUser.Username = model.Username;
            }

            if (model.DoB.HasValue)
            {
                var date = model.DoB.Value.ToShortDateString();
                existingUser.DoB = DateOnly.Parse(date);
            }
            else
            {
                existingUser.DoB = existingUser.DoB;
            }
            if (model.Address.Length > 0) { existingUser.Address = model.Address; }
            if(model.FullName.Length > 0) { existingUser.FullName  = model.FullName; }
            if(isPhoneValid == null) { existingUser.Phone = model.Phone; }
            
            try
            {
                _userRepo.Update(existingUser);
                return new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Data = existingUser,
                    Message = "Update successfully!",
                };
            }
            catch (Exception ex) {
                return new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Data = existingUser,
                    Message = ex.Message,
                };
            }
            




        }

        private async Task<string> GenerateID()
        {
            var userList = await _userRepo.GetAllUser();
            int userLength = userList.Count() + 1;
            return "USR" + userLength.ToString();
        }
        private string GenerateUsername(string fullname, string id)
        {
            string[] nameList = fullname.Split(' ');
            string name = nameList.Last();
            return name + id;
        }

    }
}
