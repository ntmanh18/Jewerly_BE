using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.ResultModel;
using Data.Model.WarrantyModel;
using Data.Repository.CustomerRepo;
using Data.Repository.ProductRepo;
using Data.Repository.UserRepo;
using Data.Repository.VoucherRepo;
using Data.Repository.WarrantyRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bussiness.Services.WarrantyService
{
    public class WarrantyService : IWarrantyService
    {
        private readonly IWarrantyRepo _warrantyRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;
        public WarrantyService( ICustomerRepo customerRepo, IProductRepo productRepo, IWarrantyRepo warrantyRepo, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService)
        {
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _warrantyRepo = warrantyRepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
        }

        public async Task<ResultModel> CreateWarranty(string? token, WarrantyCreateModel warrantyCreate)
        {
            var resultModel = new ResultModel
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
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";
                return resultModel;
            }
            var customerExist = await _customerRepo.GetCustomerById(warrantyCreate.CustomerId);
            if (customerExist == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"Customer with ID {warrantyCreate.CustomerId} does not exist.";
                return resultModel;
            }
            var productExist = await _productRepo.GetProductByIdv2(warrantyCreate.ProductId);
            if (productExist == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"Product with ID {warrantyCreate.ProductId} does not exist.";
                return resultModel;
            }
            DateOnly startDate = new DateOnly(warrantyCreate.StartDate.Year, warrantyCreate.StartDate.Month, warrantyCreate.StartDate.Day);
            DateOnly now = DateOnly.FromDateTime(DateTime.Today);
            if (startDate< now)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = "The start date must be today or later.";
                return resultModel;
            }
            if (warrantyCreate.Desc != "One-year warranty" && warrantyCreate.Desc != "Half-year warranty" && warrantyCreate.Desc != "Two-year warranty")
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = "The description must be either 'One-year warranty', 'Half-year warranty', or 'Two-year warranty'.";
                return resultModel;
            }
            var lastWarranty = await _warrantyRepo.GetLastWarrantyAsync();
            int number = 1;
            if (lastWarranty != null)
            {
                if (int.TryParse(lastWarranty.WarrantyId.Substring(1), out int lastIdNumber))
                {
                    number = lastIdNumber + 1;
                }
            }
            var warranty = new Warranty
            {
                WarrantyId = $"W{number:000}",
                CustomerCustomerId = warrantyCreate.CustomerId,
                StartDate = startDate,
                Desc = warrantyCreate.Desc,
                ProductId = warrantyCreate.ProductId,
            };
            await _warrantyRepo.CreateWarrantyAsync(warranty);
            var createdWarrantyWithInlude = await _warrantyRepo.GetWarrantyByIdWithIncludesAsync(warranty.WarrantyId);
            resultModel.Data = createdWarrantyWithInlude;
            resultModel.IsSuccess = true;
            resultModel.Code = (int)HttpStatusCode.OK;
            resultModel.Message = "Warranty created successfully.";
            return resultModel;
        }
    }
}
