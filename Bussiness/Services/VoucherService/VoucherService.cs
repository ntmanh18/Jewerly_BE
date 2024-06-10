using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.GemModel;
using Data.Model.ResultModel;
using Data.Model.VoucherModel;
using Data.Repository.CustomerRepo;
using Data.Repository.GemRepo;
using Data.Repository.UserRepo;
using Data.Repository.VoucherRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.VoucherService
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepo _voucherRepo;
        private readonly IUserRepo _userRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;
        public VoucherService(IUserRepo userRepo, ICustomerRepo customerRepo,IVoucherRepo voucherRepo, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService)
        {
            _userRepo = userRepo;
            _customerRepo = customerRepo;
            _voucherRepo = voucherRepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
        }

        public async Task<ResultModel> CreateVoucher(string token, VoucherCreateModel voucherCreate)
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
            var userExists = await _userRepo.GetByIdAsync(voucherCreate.CreatedBy);
            if (userExists == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"User with ID {voucherCreate.CreatedBy} does not exist.";
                return resultModel;
            }
            var customerExists = await _customerRepo.GetCustomerById(voucherCreate.CustomerCustomerId);
            if (customerExists == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"Customer with ID {voucherCreate.CustomerCustomerId} does not exist.";
                return resultModel;
            }

            DateOnly expiredDay = new DateOnly(voucherCreate.ExpiredDay.Year, voucherCreate.ExpiredDay.Month, voucherCreate.ExpiredDay.Day);
            DateOnly publishedDay = DateOnly.FromDateTime(DateTime.Today);
            if (expiredDay < publishedDay)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = "ExpiredDay cannot be earlier than PublishedDay.";
                return resultModel;
            }
            var voucher = new Voucher
            {
                CreatedBy = voucherCreate.CreatedBy,
                ExpiredDay = expiredDay,
                PublishedDay = publishedDay,
                Cost = voucherCreate.Cost,
                CustomerCustomerId = voucherCreate.CustomerCustomerId,
            };
            await _voucherRepo.CreateVoucherAsync(voucher);
            resultModel.Data = voucher;
            resultModel.IsSuccess = true;
            resultModel.Code = (int)HttpStatusCode.OK;
            resultModel.Message = "Voucher created successfully.";
            return resultModel;
        }
        
    }
}
