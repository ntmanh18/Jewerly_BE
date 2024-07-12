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
using Microsoft.IdentityModel.Tokens;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bussiness.Services.VoucherService
{
    public class VoucherService : IVoucherService
    {
        private readonly JewerlyV6Context _context;
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

        public async Task<ResultModel> CreateVoucher(string? token, VoucherCreateModel voucherCreate)
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

            var customerExists = await _customerRepo.GetCustomerById(voucherCreate.CustomerCustomerId);
            if (customerExists == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"Customer with ID {voucherCreate.CustomerCustomerId} does not exist.";
                return resultModel;
            }
            if (voucherCreate.Cost > 1 || voucherCreate.Cost <0)
            {

                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "Cost must be in range 0-1";
                return resultModel;
            }
            DateOnly expiredDay = new DateOnly(voucherCreate.ExpiredDay.Year, voucherCreate.ExpiredDay.Month, voucherCreate.ExpiredDay.Day);
            DateOnly publishedDay = new DateOnly(voucherCreate.PublishedDay.Year, voucherCreate.PublishedDay.Month, voucherCreate.PublishedDay.Day);
            DateOnly now = DateOnly.FromDateTime(DateTime.Today);
            if (expiredDay < publishedDay && publishedDay < now)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = "ExpiredDay cannot be earlier than PublishedDay or PublishedDay cannot be earlier than now";
                return resultModel;
            }
            var lastVoucher = await _voucherRepo.GetLastVoucherAsync();
            int number = 1;
            if (lastVoucher != null)
            {
                if (int.TryParse(lastVoucher.VoucherId.Substring(1), out int lastIdNumber))
                {
                    number = lastIdNumber + 1;
                }
            }
            var voucher = new Voucher
            {
                VoucherId = $"V{number:000}",
                CreatedBy = decodeModel.userid,
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

        public async Task<ResultModel> DeleteVoucherAsync(string? token, string voucherId)
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
            try
            {
                var voucher = await _voucherRepo.GetVoucherByIdAsync(voucherId);
                if (voucher == null)
                {
                    resultModel.IsSuccess = false;
                    resultModel.Code = (int)HttpStatusCode.NotFound;
                    resultModel.Message = "Voucher not found.";
                    return resultModel;
                }
                await _voucherRepo.DeleteVoucherAsync(voucher);
                resultModel.Data = voucher;
                resultModel.Message = "Voucher deleted successfully.";
            }
            catch (Exception ex)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.InternalServerError;
                resultModel.Message = $"Error deleting voucher: {ex.Message}";
            }

            return resultModel;
        }

        public async Task<ResultModel> UpdateVoucher(string? token, VoucherRequestModel voucherUpdate)
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
            var voucherExists = await _voucherRepo.GetVoucherByIdAsync(voucherUpdate.VoucherId);
            if (voucherExists == null)
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request voucher not found";
                return resultModel;
            }
            if (string.IsNullOrEmpty(voucherUpdate.CreatedBy))
            {
                voucherUpdate.CreatedBy = voucherExists.CreatedBy;
            }
            if (string.IsNullOrEmpty(voucherUpdate.CustomerCustomerId))
            {
                voucherUpdate.CustomerCustomerId = voucherExists.CustomerCustomerId;
            }
            if (voucherUpdate.Cost < 0)
            {
                voucherUpdate.Cost = voucherExists.Cost;
            }
            var userExists = await _userRepo.GetByIdAsync(voucherUpdate.CreatedBy);
            if (userExists == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"User with ID {voucherUpdate.CreatedBy} does not exist.";
                return resultModel;
            }
            var customerExists = await _customerRepo.GetCustomerById(voucherUpdate.CustomerCustomerId);
            if (customerExists == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"Customer with ID {voucherUpdate.CustomerCustomerId} does not exist.";
                return resultModel;
            }
            DateOnly expiredDay = new DateOnly(voucherUpdate.ExpiredDay.Year, voucherUpdate.ExpiredDay.Month, voucherUpdate.ExpiredDay.Day);
            //so sánh expiređay và punlisheDay
            if (voucherUpdate.ExpiredDay.Year == 0 && voucherUpdate.ExpiredDay.Month == 0 && voucherUpdate.ExpiredDay.Day == 0)
            {
                expiredDay = voucherExists.ExpiredDay;
            }
            if (expiredDay <= voucherExists.PublishedDay)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = "ExpiredDay cannot be earlier than PublishedDay";
                return resultModel;
            }
            Voucher voucher = new Voucher
            {
                VoucherId = voucherUpdate.VoucherId,
                CreatedBy = voucherUpdate.CreatedBy,
                ExpiredDay = expiredDay,
                Cost = voucherUpdate.Cost,
                CustomerCustomerId = voucherUpdate.CustomerCustomerId,
            };
            await _voucherRepo.UpdateVoucherAsync(voucher);
            var updateVoucherWithIncludes = await _voucherRepo.GetVoucherByIdWithIncludesAsync(voucher.VoucherId);
            resultModel.Data = updateVoucherWithIncludes;
            resultModel.Message = "Voucher updated successfully.";
            return resultModel;
        }

        public async Task<ResultModel> ViewListVoucher(string? token, VoucherSearchModel voucherSearch)
        {
            var resultModel = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";
                return resultModel;
            }
            var query = _voucherRepo.GetVoucherQuery();
            var searchDay = voucherSearch.expiredDay;
            if (searchDay.HasValue)
            {
                var searchDayValue = DateOnly.FromDateTime(searchDay.Value);
                query = query.Where(v => v.ExpiredDay >= searchDayValue);
            }

            if (!string.IsNullOrEmpty(voucherSearch.customerId))
            {
                query = query.Where(v => v.CustomerCustomer.CustomerId == voucherSearch.customerId);
            }

            if (!string.IsNullOrEmpty(voucherSearch.customerName))
            {
                query = query.Where(v => v.CustomerCustomer.FullName.Contains(voucherSearch.customerName));
            }

            if (!string.IsNullOrEmpty(voucherSearch.customerPhone))
            {
                query = query.Where(v => v.CustomerCustomer.Phone.Contains(voucherSearch.customerPhone));
            }
            
            if (!string.IsNullOrEmpty(voucherSearch.customerEmail))
            {
                query = query.Where(v => v.CustomerCustomer.Email.Contains(voucherSearch.customerEmail));
            }

            var vouchers = await query.ToListAsync();
            resultModel.Data = vouchers;

            return resultModel;
        }
    }
}
