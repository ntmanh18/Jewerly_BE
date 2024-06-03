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
using System.Collections;
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

        public async Task<ResultModel> ChangePassword(string token, ChangePasswordModel model)
        {
            var res = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            

            var existingUser = await _userRepo.GetByIdAsync(decodeModel.userid);
            bool isMatch = HashPass.VerifyPassword(model.OldPassword, existingUser.Password);
            if (!isMatch)
            {
                res.IsSuccess = false;
                res.Code = 400;
                res.Message = "Old password is wrong";
                return res;
            }
            try {
            try
            {

                string hashNewPassword = HashPass.HashPassword(model.NewPassword);
                existingUser.Password = hashNewPassword;
                await _userRepo.Update(existingUser);
                

                res.IsSuccess = true;
                res.Code = 200;
                res.Message = "Change password succesfully";
                return res;
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Code = 400;
                return res;
            }



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
            if (!(model.Role == 1 || model.Role == 2 || model.Role == 3))
            {

                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Role is not valid";

                return res;
            }

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


        public async Task<ResultModel> DeactiveUser(string token, DeactiveUserReqModel model)
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

            var existingUser = await _userRepo.GetByIdAsync(model.UserId);
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
             if(existingUser.Status == true) { existingUser.Status = false; } else { existingUser.Status = true; }
            if (existingUser.Status == true) { existingUser.Status = false; } else { existingUser.Status = true; }
            try
            {
                var result = await _userRepo.Update(existingUser);

                return new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Data = null,
                    Message = existingUser.Status == true
                    ? "Active user successful"
                    : "Deactive user successful",
                };
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message,
                };
            }


        }

        public async Task<ResultModel> UpdateRole(string token, UpdateRoleReqModel model)
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
            if(!(model.Role ==1 || model.Role ==2 || model.Role == 3)) {
            if (!(model.Role == 1 || model.Role == 2 || model.Role == 3))
            {

                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Role is not valid";

                return res;
            }
            var existingUser = await _userRepo.GetByIdAsync(model.Id);
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
            existingUser.Role = model.Role;
            try
            {
                await _userRepo.Update(existingUser);
                return new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Data = existingUser,
                    Message = "Update role successfully",
                };
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = ex.Message,
                };
            }
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
            var existingUser = await _userRepo.GetByIdAsync(decodeModel.userid);
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

            if (model.Username.Length > 0)
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
            
            if (model.FullName.Length > 0) { existingUser.FullName = model.FullName; }
            if (isPhoneValid == null) { existingUser.Phone = model.Phone; }

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
            catch (Exception ex)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.InternalServerError,
                    Data = existingUser,
                    Message = ex.Message,
                };
            }
            





        }

        public async Task<ResultModel> ViewUserList(string token, UserQueryObject query)
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
            var users = await _userRepo.GetAllUserQuery();
            if(query.role != 0)
            if (query.role != 0)
            {
                users = users.Where(x => x.Role == query.role).ToList();
            }
            if(query.status.HasValue)
            if (query.status.HasValue)
            {
                users = users.Where(x => x.Status == query.status).ToList();
            }
            if (!string.IsNullOrWhiteSpace(query.name))
            {
                users = users.Where(x => x.FullName.Contains(query.name)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(query.id))
            {
                users = users.Where(x => x.UserId.Contains(query.id)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(query.sortBy))
            {
                if (query.sortBy.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    users= query.isDescending ? users.OrderByDescending(s => s.UserId).ToList() : users.OrderBy(s => s.UserId).ToList();
                    users = query.isDescending ? users.OrderByDescending(s => s.UserId).ToList() : users.OrderBy(s => s.UserId).ToList();
                }
                if (query.sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    users = query.isDescending ? users.OrderByDescending(s => s.FullName).ToList() : users.OrderBy(s => s.FullName).ToList();
                }


            }

            ViewUserResModel user = new ViewUserResModel();
            users.Select(s => new ViewUserResModel
            {
                UserId = s.UserId,
                FullName = s.FullName,
                Username = s.Username,
                Role = s.Role,
                DoB = s.DoB,
                Phone = s.Phone,
                Address = s.Address,
                Status = s.Status


            });
               
            



            res.IsSuccess = true;
            res.Code = (int)HttpStatusCode.OK;
            res.Data = users;
            return res;
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
