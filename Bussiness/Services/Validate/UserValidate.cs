using Data.Model.ResultModel;
using Data.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bussiness.Services.Validate
{
    public class UserValidate : IUserValidate
    {
        private readonly IUserRepo _userRepo;
        public UserValidate(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ResultModel> IsPhoneValid(string phone)
        {
            var res = new ResultModel();

            try
            {
                if (!Regex.IsMatch(phone, "^0\\d{9,10}$"))
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.BadRequest;
                    res.Message = "Invalid phone number";
                    return res;

                }
                var userList = await _userRepo.GetAllUser();
                var existedPhone = userList.FirstOrDefault(x => x.Phone == phone);
                if (existedPhone != default)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.BadRequest;
                    res.Message = "The provided phone has already existed";
                    return res;
                }
                return null;


            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.BadRequest;
                res.Message = ex.Message;
                return res;
            }
            return null;
        }

        public async Task<ResultModel> IsUserNameUnique(string username)
        {
            var result = new ResultModel();
            try
            {
                var existedUser = await _userRepo.GetByUsernameAsync(username);
                if (existedUser != null)
                {
                    result.IsSuccess = false;
                    result.Code = (int)HttpStatusCode.BadRequest;
                    result.Message = "The provided username has already existed";
                }
                return result;

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = (int)HttpStatusCode.BadRequest;
                result.Message = ex.Message;
                return result;
            }

        }
    }
}
