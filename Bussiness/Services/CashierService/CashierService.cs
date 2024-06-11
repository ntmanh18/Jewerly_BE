using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.CashierModel;
using Data.Model.CustomerModel;
using Data.Model.ResultModel;
using Data.Repository.CashierRepo;
using Data.Repository.CustomerRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.CashierService
{
    public class CashierService : ICashierService
    {

        private static Random _random = new Random();
        private readonly ICashierRepo _cashierRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;

        public CashierService(
            ICashierRepo cashierRepo,
            IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService
            )
        {
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
            _cashierRepo = cashierRepo;
        }

        public async Task<ResultModel> CreateCashier(string? token, CashierRequestModel cashierModel)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            
            var allUser = GetCashiers().Result;
            var overlappingCashiers = GetOverlappingCashiers(cashierModel.StartCash, cashierModel.EndCash, allUser, cashierModel);


            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var user = await _cashierRepo.GetUserById(cashierModel.UserId);
            if (!IsMoreThanFourHoursDifference(cashierModel.EndCash, cashierModel.StartCash))
            {
                resultModel.IsSuccess = false;
                resultModel.Code = 400;
                resultModel.Message = "ca làm phải nhiều hơn 4 giờ";
            }
            else if(user == null){
                resultModel.IsSuccess = false;
                resultModel.Code = 400;
                resultModel.Message = "Không tìm thấy User";
            }
            else if (user.Role != 1)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = 400;
                resultModel.Message = "User phải có role là Staff";
            }
            else if (cashierModel.CashNumber != 1 && cashierModel.CashNumber != 2 && cashierModel.CashNumber != 3 && cashierModel.CashNumber != 4)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = 400;
                resultModel.Message = "phải nhập Cash Number từ 1-4";
            }
            else if (overlappingCashiers.Any())
            {
                foreach (var user1 in overlappingCashiers)
                {
                        resultModel.IsSuccess = false;
                        resultModel.Code = 400;
                        resultModel.Message = "User trùng lịch làm";
                }
            }
            else
            {
                try
                {
                    Cashier cashier = new Cashier
                    {
                        CashId = GenerateCustomerId(),
                        StartCash = cashierModel.StartCash,
                        EndCash = cashierModel.EndCash,
                        Income = cashierModel.Income,
                        CashNumber = cashierModel.CashNumber,
                        UserId = cashierModel.UserId,
                        User = user,

                    };
                    await _cashierRepo.CreateCashier(cashier);
                    resultModel.IsSuccess = true;
                    resultModel.Code = 200;
                    resultModel.Data = cashier;
                    resultModel.Message = "Correct";

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            
            return resultModel;
        }


        public static string GenerateCustomerId()
        {

            int randomNumber = _random.Next(0, 10000);
            string numberPart = randomNumber.ToString("D5");
            return "CH" + numberPart;
        }
        public async Task<User> GetUserById(string customerId)
        {
            return await _cashierRepo.GetUserById(customerId);
        }

        static bool IsMoreThanFourHoursDifference(DateTime dt1, DateTime dt2)
        {
            TimeSpan difference = dt1 - dt2;
            return difference.TotalHours > 4;
        }

        public static List<User> GetOverlappingCashiers(DateTime startTime, DateTime endTime, List<Cashier> cashiers, CashierRequestModel cashierModel)
        {
            var overlappingCashiers = cashiers
                .Where(c => startTime < c.EndCash && endTime > c.StartCash && c.Status == 1 && c.UserId == cashierModel.UserId)
                .Select(c => c.User)
                .ToList();

            return overlappingCashiers;
        }
        public static List<User> GetOverlappingCashiersUpdate(DateTime startTime, DateTime endTime, List<Cashier> cashiers, CashierUpdateModel cashierModel)
        {
            var overlappingCashiers = cashiers
                .Where(c => startTime < c.EndCash && endTime > c.StartCash && c.Status == 1 && c.UserId == cashierModel.UserId)
                .Select(c => c.User)
                .ToList();

            return overlappingCashiers;
        }
        public async Task<List<Cashier>> GetCashiers()
        {

            var cashiers = await _cashierRepo.GetCashiers();
            List<Cashier> updatedCashiers = new List<Cashier>();

            foreach (var cashier in cashiers)
            {
                Cashier cashier1 = new Cashier
                {
                    CashId =  cashier.CashId,
                    StartCash = cashier.StartCash,
                    EndCash = cashier.EndCash,
                    Income = cashier.Income,
                    CashNumber = cashier.CashNumber,
                    UserId = cashier.UserId,
                    Status = cashier.Status,
                    User = cashier.User,

                };
                updatedCashiers.Add(cashier1);

            }

            return updatedCashiers;
        }

        public async Task<ResultModel> GetAllCashiers(string token)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var cashiers = await _cashierRepo.GetAllCashiers();
            List<Cashier> updatedCashiers = new List<Cashier>();

            foreach (var cashier in cashiers)
            {
                Cashier cashier1 = new Cashier
                {
                    CashId = cashier.CashId,
                    StartCash = cashier.StartCash,
                    EndCash = cashier.EndCash,
                    Income = cashier.Income,
                    CashNumber = cashier.CashNumber,
                    UserId = cashier.UserId,
                    Status= cashier.Status,
                };
                updatedCashiers.Add(cashier1);

            }
            resultModel.Code = 200;
            resultModel.IsSuccess = true;
            resultModel.Data = updatedCashiers;
            return resultModel;
        }

        public async Task<ResultModel> GetCashierById(string id)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };
            IEnumerable<Cashier> cashiers = await _cashierRepo.GetAllCashiers();
            List<Cashier> updatedCashiers = new List<Cashier>();
            foreach (var cashier in cashiers)
            {
                Cashier cashier1 = new Cashier
                {
                    CashId = cashier.CashId,
                    StartCash = cashier.StartCash,
                    EndCash = cashier.EndCash,
                    Income = cashier.Income,
                    CashNumber = cashier.CashNumber,
                    UserId = cashier.UserId,
                    Status = cashier.Status,
                };
                updatedCashiers.Add(cashier1);

            }

            if (String.IsNullOrEmpty(id))
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Please enter any id";

            }

            else
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;

                for (int i = 0; i < updatedCashiers.Count; i++)
                {
                    if (updatedCashiers[i].CashId != id)
                    {
                        updatedCashiers.RemoveAll(c => c.CashId == updatedCashiers[i].CashId);
                        i = i - 1;
                    }

                }
                if (updatedCashiers.Count() > 0)
                {
                    resultModel.Message = "Cashier found";
                }
                else
                {
                    resultModel.Message = "Not found";
                }
                IEnumerable<Cashier> updatedCashiers2 = updatedCashiers;
                resultModel.Data = updatedCashiers2;
            }
            return resultModel;

        }

        public async Task<ResultModel> UpdateCashier(string? token, CashierUpdateModel cashierModel)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var existingProduct = await GetCashierById(cashierModel.CashId);
            var allUser = GetCashiers().Result;
            var overlappingCashiers = GetOverlappingCashiersUpdate(cashierModel.StartCash, cashierModel.EndCash, allUser, cashierModel);
            if (existingProduct.Message == "Not found")
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request cashier not found";
            }
            else if (existingProduct.Message == "Cashier found")
            {
                var user = await _cashierRepo.GetUserById(cashierModel.UserId);
                if (!IsMoreThanFourHoursDifference(cashierModel.EndCash, cashierModel.StartCash))
                {
                    resultModel.IsSuccess = false;
                    resultModel.Code = 400;
                    resultModel.Message = "ca làm phải nhiều hơn 4 giờ";
                }
                else if (user == null)
                {
                    resultModel.IsSuccess = false;
                    resultModel.Code = 400;
                    resultModel.Message = "Không tìm thấy User";
                }
                else if (user.Role != 1)
                {
                    resultModel.IsSuccess = false;
                    resultModel.Code = 400;
                    resultModel.Message = "User phải có role là Staff";
                }
                else if (cashierModel.CashNumber != 1 && cashierModel.CashNumber != 2 && cashierModel.CashNumber != 3 && cashierModel.CashNumber != 4)
                {
                    resultModel.IsSuccess = false;
                    resultModel.Code = 400;
                    resultModel.Message = "phải nhập Cash Number từ 1-4";
                }
                else if (overlappingCashiers.Any())
                {
                    foreach (var user1 in overlappingCashiers)
                    {
                        resultModel.IsSuccess = false;
                        resultModel.Code = 400;
                        resultModel.Message = "User trùng lịch làm";
                    }
                }
                else
                {


                    resultModel.Code = 200;
                    resultModel.IsSuccess = true;
                    resultModel.Message = "Update success";
                    Cashier cashier = new Cashier
                    {
                        CashId = cashierModel.CashId,
                        StartCash = cashierModel.StartCash,
                        EndCash = cashierModel.EndCash,
                        Income = cashierModel.Income,
                        CashNumber = cashierModel.CashNumber,
                        UserId = cashierModel.UserId,
                        User = user,
                    };
                    var cashierUpdate = await _cashierRepo.UpdateCashier(cashier);

                    resultModel.Data = cashierUpdate;
                }
            }
            return resultModel;
        }
    }
}
