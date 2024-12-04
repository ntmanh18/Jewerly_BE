using Bussiness.Services.AccountService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.DiscountModel;
using Data.Model.ResultModel;
using Data.Repository.DiscountProductRepo;
using Data.Repository.DiscountRepo;
using Data.Repository.ProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.DiscountProductService
{
    public class DiscountProductService : IDiscountProductService
    {
        private readonly IAccountService _accountService;
        private readonly IToken _token;
        private readonly IDiscountRepo _discountRepo;
        private readonly IProductRepo _productRepo;
        private readonly IDiscountProductRepo _discountProductRepo;
        public DiscountProductService(IToken token,
            IAccountService accountService,
            IDiscountRepo discountRepo,
            IProductRepo productRepo,
            IDiscountProductRepo discountProductRepo
           )
        {
            _discountRepo = discountRepo;
            _token = token;
            _accountService = accountService;
            _productRepo = productRepo;
            _discountProductRepo = discountProductRepo;

        }
        public async Task<ResultModel> CreateDiscountProduct(string token, CreateDiscountProductReqModel req)
        {
            var res = new ResultModel
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
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";
                return res;
            }
            var Discount = await _discountRepo.GetDiscountById(req.DiscountId);
            if (Discount == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.NotFound;
                res.Message = "Discount is not existed";
                return res;
            }
            var product = await _productRepo.GetProductByIdv2(req.ProductId);
            if (Discount == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.NotFound;
                res.Message = "Product is not existed";
                return res;
            }

            if( Discount.ExpiredDay.CompareTo(DateOnly.FromDateTime(DateTime.Now))  < 0 ) {

                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.NotFound;
                res.Message = "Cannot apply expired discount to product";
                return res;
            }
            if(Discount.Cost > product.Price/2)
            {
                res.IsSuccess = false;
                res.Code= (int)HttpStatusCode.Forbidden;
                res.Message = "Discount can not be > 50% of product";
                return res;
            }
            DiscountProduct discountProduct = new DiscountProduct()
            {
                DiscountDiscountId = req.DiscountId,
                ProductProductId = product.ProductId,
            };
            try
            {
                await _discountProductRepo.CreateDiscountProduct(discountProduct);
                res.IsSuccess = true;
                res.Code = (int)HttpStatusCode.OK;
                res.Message = "Add discount to product successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Code= (int)HttpStatusCode.BadRequest;
                res.Message = ex.Message;
                return res;
            }
        }

        public async Task<ResultModel> DeleteDiscountProduct(string token, CreateDiscountProductReqModel req)
        {
            var res = new ResultModel
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
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";
                return res;
            }
            var Discount = await _discountRepo.GetDiscountById(req.DiscountId);
            if (Discount == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.NotFound;
                res.Message = "Discount is not existed";
                return res;
            }
            var product = await _productRepo.GetProductByIdv2(req.ProductId);
            if (Discount == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.NotFound;
                res.Message = "Product is not existed";
                return res;
            }

            var discountProduct = await _discountProductRepo.GetUniqueDiscountProduct(req.DiscountId,req.ProductId);
            if (discountProduct == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.NotFound;
                res.Message = "This discount has not been applied to the product";
                return res;
            }
            
            try
            {
                await _discountProductRepo.DeleteDiscountProduct(discountProduct);
                res.IsSuccess = true;
                res.Code = (int)HttpStatusCode.OK;
                res.Message = "Remove discount from product successfully";
                return res;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.BadRequest;
                res.Message = ex.Message;
                return res;
            }
        }
    }
}
