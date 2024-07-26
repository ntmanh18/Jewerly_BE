using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Bussiness.Services.Validate;
using Data.Entities;
using Data.Model.DiscountModel;
using Data.Model.ResultModel;
using Data.Repository.DiscountProductRepo;
using Data.Repository.DiscountRepo;
using Data.Repository.ProductRepo;
using Data.Repository.UserRepo;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bussiness.Services.DiscountService
{
    public class DiscountService : IDiscountService
    {
        private readonly IAccountService _accountService;
        private readonly IToken _token;
        private readonly IDiscountRepo _discountRepo;
        private readonly IProductRepo _productRepo;
        private readonly IDiscountProductRepo _discountProductRepo;
        public DiscountService(IToken token,
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

        public async Task<ResultModel> CreateDiscount(string token, CreateDiscountReqModel req)
        {
            var res = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() {2});
            if (!isValidRole)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";
                return res;
            }
            var a = DateOnly.FromDateTime(req.PublishDay);
            var b = DateOnly.FromDateTime(DateTime.Now.AddHours(1));
            var isValidPubDate = a.CompareTo(b);
            if(isValidPubDate < 0)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Publish Day can not be earlier than today";
                return res;
            }
            var isValidDate = req.ExpiredDay.CompareTo(req.PublishDay);
            if(isValidDate < 0 )
            {
                res.IsSuccess = false;
                res.Code= (int)HttpStatusCode.Forbidden;
                res.Message = "Expired Day must be later than publish day";
                return res;
            }
            if (req.Cost < 10000)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Cost must be equal to or greater than 10000";
                return res;
            }
            
           
            var id = await GenerateIDAsync(req.ExpiredDay);
            Discount discount = new Discount
            {
                DiscountId = id,
                CreatedBy = decodeModel.userid,
                ExpiredDay = DateOnly.FromDateTime(req.ExpiredDay),
                PublishDay = DateOnly.FromDateTime(req.PublishDay),
                Cost = req.Cost,

            };
            var data = new
            {
                Id = discount.DiscountId,
                CreatedBy = discount.CreatedBy,
                Expireday = discount.ExpiredDay,
                PublishDay=discount.PublishDay,
                Cost = discount.Cost,
            };
            try {
                await _discountRepo.Insert(discount);
                res.IsSuccess = true;
                res.Code = (int)HttpStatusCode.OK;
                res.Data = data;
                return res;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.ToString();
                return res;
            }
           

        }
        private async Task<string> GenerateIDAsync(DateTime expDate)
        {
            List<Discount> discounts = await _discountRepo.GetAllDiscount();
            string res ="D"+ expDate.ToShortDateString() + discounts.Count.ToString();
            return res;
        }

        public async Task<ResultModel> GetAllDiscount(string token, DiscountQueryModel query)
        {
            var res = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = null,
                Message = null,
            };

            var decodeModel = _token.decode(token);
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() {1, 2 });
            if (!isValidRole)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";

                return res;
            }
            List<Discount> discounts = await _discountRepo.GetAllDiscount();
            if(!query.id.IsNullOrEmpty())
            {
                discounts = discounts.Where(c => c.DiscountId.Contains(query.id)).ToList();
            }
            if(!(query.productId.IsNullOrEmpty()))
            {
                var existedProduct = _productRepo.GetProductByIdv2(query.productId);
                if (existedProduct == null) {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Product is not existed";
                    return res;
                }
                else
                {
                  discounts = discounts.Where(c => c.DiscountProducts.ToList().Exists(c => c.ProductProduct.ProductId == query.productId)).ToList();
                }
            }
            var now = DateOnly.FromDateTime(DateTime.Now);

            if (query.status == true)
            {
                discounts= discounts.Where(c=> c.ExpiredDay.CompareTo(now)>=0).ToList();   
            }
            else
            {
                discounts = discounts.Where(c => c.ExpiredDay.CompareTo(now) < 0).ToList();
            }
            List<string> pList = new List<string>();
            
            var data = discounts.Select(c => new ViewDiscountResModel
            {
                DiscountId = c.DiscountId,
                CreatedBy = c.CreatedByNavigation.Username,
                ExpiredDay = c.ExpiredDay,
                PublishDay = c.PublishDay,
                Cost = c.Cost,
            });
            res.IsSuccess = true;
            res.Code = (int)HttpStatusCode.OK;
            res.Data = data;
            return res;

        }

        public async Task<ResultModel> UpdateDiscount(string token, UpdateDiscountReqModel req)
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
            var oldDiscount =await _discountRepo.GetDiscountById(req.discountId);
            if(oldDiscount == null)
            {
                res.IsSuccess = false;
                res.Code= (int)HttpStatusCode.NotFound;
                res.Message = "Discount is not existed";
                return res;
            }
           
            var isValidUpdatePubDate = oldDiscount.PublishDay.CompareTo(DateOnly.FromDateTime(DateTime.Now));
            if (isValidUpdatePubDate < 0)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Can not update in use discount";
                return res;
            }
            var a = DateOnly.FromDateTime(req.PublishDay);
            var b = DateOnly.FromDateTime(DateTime.Now.AddHours(1));
            var isValidPubDate = a.CompareTo(b);
            if (isValidPubDate < 0)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Publish Day can not be earlier than today";
                return res;
            }
            var isValidDate = req.ExpiredDay.CompareTo(req.PublishDay);
            if (isValidDate < 0)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Expired Day must be later than publish day";
                return res;
            }
            if (req.Cost < 10000)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Cost must be equal to or greater than 10000";
                return res;
            }

            oldDiscount.ExpiredDay = DateOnly.FromDateTime(req.ExpiredDay);
            oldDiscount.PublishDay = DateOnly.FromDateTime(req.PublishDay);
            oldDiscount.Cost = req.Cost;
            try
            {
                await _discountRepo.Update(oldDiscount);
                res.IsSuccess = true;
                res.Code = (int)HttpStatusCode.OK;
                res.Message = "Update discount successfully";
                res.Data = req;
                return res;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Data = ex.ToString();
                return res;
            }

        }

        public async Task<ResultModel> DeleteDiscount(string token, string discountId)
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
            var oldDiscount = await _discountRepo.GetDiscountById(discountId);
            if (oldDiscount == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.NotFound;
                res.Message = "Discount is not existed";
                return res;
            }

            var discountProduct = await _discountProductRepo.GetDiscountProductByDiscout(discountId);
            if (discountProduct != null) {

                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Can not delete discount applying to Product";
                return res;
            }
            try {
                await _discountRepo.Remove(oldDiscount);
                res.IsSuccess = true;
                res.Code = (int)HttpStatusCode.OK;
                res.Message = "Delete discount successfully";
                return res;
            }catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Code= (int)HttpStatusCode.BadRequest;
                res.Message = ex.Message;
                return res;
            }
           
        }
    }
}
