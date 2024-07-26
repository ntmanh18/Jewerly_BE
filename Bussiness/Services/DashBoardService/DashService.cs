using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Model.ResultModel;
using Data.Repository.DashBoardRepo;
using Data.Repository.GemRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.DashBoardService
{
    public class DashService : IDashService
    {
        private readonly IDashRepo _dashRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;
        public DashService(IDashRepo dashRepo, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService)
        {
            _dashRepo = dashRepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
        }

        public async Task<ResultModel> GetIncomeByCashNumberAsync(string? token)
        {
            var resultModel = new ResultModel
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
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            try
            {
                var incomeByCashNumber = await _dashRepo.GetIncomeByCashNumberAsync();
                resultModel.Data = incomeByCashNumber.Select(group => new
                {
                    CashNumber = group.Key,
                    //TotalIncome = group.Sum(c => c.Income ?? 0)
                    TotalIncome = group.Sum(c => c.Income)
                }).ToList();
                resultModel.Message = "Income by cash number retrieved successfully.";
            }
            catch (Exception ex)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.InternalServerError;
                resultModel.Message = ex.Message;
            }

            return resultModel;
        }

        public async Task<ResultModel> GetIncomeByMonthAsync(string? token)
        {
            var resultModel = new ResultModel
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
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't have permission to perform this action.";
                return resultModel;
            }

            try
            {
                var incomeByMonth = await _dashRepo.GetIncomeByMonthAsync();

                if (incomeByMonth != null && incomeByMonth.Any())
                {
                    resultModel.Data = incomeByMonth.Select(dto => new
                    {
                        dto.Year,
                        dto.Month,
                        dto.TotalIncome
                    }).ToList();

                    resultModel.Message = "Income by month retrieved successfully.";
                }
                else
                {
                    resultModel.Message = "No income data available for the selected period.";
                }
            }
            catch (Exception ex)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.InternalServerError;
                resultModel.Message = ex.Message;
            }

            return resultModel;
        }


        public async Task<ResultModel> GetTotalIncomeAsync(string? token)
        {
            var resultModel = new ResultModel
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
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            try
            {
                var totalIncome = await _dashRepo.GetTotalIncomeAsync();
                resultModel.Data = totalIncome;
                resultModel.Message = "Total income retrieved successfully.";
            }
            catch (Exception ex)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.InternalServerError;
                resultModel.Message = ex.Message;
            }

            return resultModel;
        }
    }
}
