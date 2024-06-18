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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2 });
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

        public async Task<ResultModel> UpdateWarranty(string? token, WarrantyUpdateModel warrantyUpdate)
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
            var warrantyExists = await _warrantyRepo.GetWarrantyByIdAsync(warrantyUpdate.WarrantyId);
            if (warrantyExists == null)
            {
                resultModel.Code = 200;
                resultModel.IsSuccess = true;
                resultModel.Message = "Request warranty not found";
                return resultModel;
            }
            if (string.IsNullOrEmpty(warrantyUpdate.CustomerId))
            {
                warrantyUpdate.CustomerId = warrantyExists.CustomerCustomerId;
            }
            if (string.IsNullOrEmpty(warrantyUpdate.ProductId))
            {
                warrantyUpdate.ProductId = warrantyExists.ProductId;
            }
            if (string.IsNullOrEmpty(warrantyUpdate.Desc))
            {
                warrantyUpdate.Desc = warrantyExists.Desc;
            }
            var customerExist = await _customerRepo.GetCustomerById(warrantyUpdate.CustomerId);
            if (customerExist == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"Customer with ID {warrantyUpdate.CustomerId} does not exist.";
                return resultModel;
            }
            var productExist = await _productRepo.GetProductByIdv2(warrantyUpdate.ProductId);
            if (productExist == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = $"Product with ID {warrantyUpdate.ProductId} does not exist.";
                return resultModel;
            }
            DateOnly startdate = new DateOnly(warrantyUpdate.StartDate.Year, warrantyUpdate.StartDate.Month, warrantyUpdate.StartDate.Day);
            DateOnly now = DateOnly.FromDateTime(DateTime.Today);
            if (warrantyUpdate.StartDate.Year == 0 && warrantyUpdate.StartDate.Month == 0 && warrantyUpdate.StartDate.Day == 0)
            {
                startdate = warrantyExists.StartDate;
            }
            else if(startdate < now)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = "The start date must be today or later.";
                return resultModel;
            }
            if (warrantyUpdate.Desc != "One-year warranty" && warrantyUpdate.Desc != "Half-year warranty" && warrantyUpdate.Desc != "Two-year warranty")
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.BadRequest;
                resultModel.Message = "The description must be either 'One-year warranty', 'Half-year warranty', or 'Two-year warranty'.";
                return resultModel;
            }
            
            Warranty warranty = new Warranty
            {
                WarrantyId = warrantyUpdate.WarrantyId,
                CustomerCustomerId = warrantyUpdate.CustomerId,
                StartDate = startdate,
                Desc = warrantyUpdate.Desc,
                ProductId = warrantyUpdate.ProductId,
            };
            await _warrantyRepo.UpdateWarrantyAsync(warranty);
            var updateWarrantyWithIncludes = await _warrantyRepo.GetWarrantyByIdWithIncludesAsync(warranty.WarrantyId);
            resultModel.Data = updateWarrantyWithIncludes;
            resultModel.Message = "Warranty updated successfully.";
            return resultModel;
        }

        public async Task<ResultModel> ViewListWarranty(string? token, WarrantySearchModel warrantySearch)
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
            var query = _warrantyRepo.GetWarrantyQuery();
            if (!string.IsNullOrEmpty(warrantySearch.warrantyId))
            {
                query = query.Where(v => v.WarrantyId == warrantySearch.warrantyId);
            }

            if (!string.IsNullOrEmpty(warrantySearch.productId))
            {
                query = query.Where(v => v.ProductId.Contains(warrantySearch.productId));
            }

            if (!string.IsNullOrEmpty(warrantySearch.productName))
            {
                query = query.Where(v => v.Product.ProductName.Contains(warrantySearch.productName));
            }

            if (!string.IsNullOrEmpty(warrantySearch.customerId))
            {
                query = query.Where(v => v.CustomerCustomerId.Contains(warrantySearch.customerId));
            }

            if (!string.IsNullOrEmpty(warrantySearch.customerName))
            {
                query = query.Where(v => v.CustomerCustomer.FullName.Contains(warrantySearch.customerName));
            }

            var warranties = await query.ToListAsync();
            resultModel.Data = warranties;
            return resultModel;
        }
    }
}
