using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.ProductGemModel;
using Data.Model.ResultModel;
using Data.Repository.GemRepo;
using Data.Repository.ProductGemRepo;
using Data.Repository.ProductRepo;
using Microsoft.IdentityModel.Tokens;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.ProductGemService
{
    public class ProductGemService : IProductGemService
    {
        private readonly IProductGemRepo _productGemRepo;
        private readonly IProductRepo _productRepo;
        private readonly IGemRepo _gemRepo;
        private readonly IToken _token;
        private readonly IAccountService _accountService;
        public ProductGemService(IProductGemRepo productGemRepo,
            IProductRepo productRepo,
            IGemRepo gemRepo,
            IToken token,
            IAccountService accountService
            )
        {
            _productGemRepo = productGemRepo;
            _productRepo = productRepo;
            _gemRepo = gemRepo;
            _token = token;
            _accountService = accountService;
        }
        public async Task<ResultModel> CreateProductGem(string token,ProductGemReqModel req)
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
            Product p = await _productRepo.GetProductByIdv2(req.ProductId);
            if (p == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Product is not existed";
                return res;
            }
            foreach ( var id in req.Gem)
            {
                Gem  gem = await _gemRepo.GetGemById(id.Key);
                var PGem =await  _productGemRepo.GetProductGemUnique(req.ProductId, id.Key);
                if (gem == null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Gem is not existed";
                    return res;
                }
                if(id.Value <= 0  )
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.BadRequest;
                    res.Message = "Invalid amount of gem";
                    return res;
                }
                else if( PGem!= null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Gem has already existed in products";
                    return res;
                }
                else
                {
                    ProductGem pg = new ProductGem()
                    {
                        ProductProductId = req.ProductId,
                        GemGemId = id.Key,
                        Amount = id.Value
                    };
                    p.Price = p.Price + gem.Price * id.Value;
                    await _productRepo.Update(p);
                    await _productGemRepo.Insert(pg);
                }
            }
            res.IsSuccess = true;
            res.Code = (int)HttpStatusCode.OK;
            res.Data = null;
            

            return res;
        }
        public async Task<ResultModel> DeleteProductGem(string token, DelteProductGemReqModel req)
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
            Product p = await _productRepo.GetProductByIdv2(req.ProductId);
            if (p == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Product is not existed";
                return res;
            }
            Gem g = await _gemRepo.GetGemById(req.GemId);
            if (g == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Gem is not existed";
                return res;
            }
            ProductGem pg = await _productGemRepo.GetProductGemUnique(req.ProductId,req.GemId);
            if (pg == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Product gem is not existed";
                return res;
            }
            try {
                p.Price = (decimal)(p.Price -( g.Price * pg.Amount));
                
                await _productGemRepo.Remove(pg);
                await _productRepo.Update(p);
                res.IsSuccess = true;
                res.Code = (int)HttpStatusCode.OK;
                res.Message = "Delete product Gem successfully";
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

        public async Task<ResultModel> UpdateProductGem(string token, ProductGemReqModel req)
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
            Product p = await _productRepo.GetProductByIdv2(req.ProductId);
            if (p == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Product is not existed";
                return res;
            }
            List<ProductGem> productgem =await _productGemRepo.GetByProduct(req.ProductId);
            if (productgem != null)
            {
                foreach (var gem in productgem)
                {
                    DelteProductGemReqModel delGem = new DelteProductGemReqModel()
                    {
                        ProductId = req.ProductId,
                        GemId = gem.GemGemId,
                    };
                    await _productGemRepo.Remove(gem);
                    p.Price = p.Price - gem.GemGem.Price;
                }
            }
            foreach (var id in req.Gem)
            {
                Gem gem = await _gemRepo.GetGemById(id.Key);
                var PGem = await _productGemRepo.GetProductGemUnique(req.ProductId, id.Key);
                if (gem == null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Gem is not existed";
                    return res;
                }
                if (id.Value <= 0)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.BadRequest;
                    res.Message = "Invalid amount of gem";
                    return res;
                }
                else if (PGem != null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Gem has already existed in products";
                    return res;
                }
                else
                {
                    ProductGem pg = new ProductGem()
                    {
                        ProductProductId = req.ProductId,
                        GemGemId = id.Key,
                        Amount = id.Value
                    };
                   
                    p.Price = p.Price + gem.Price * id.Value;
                     _productGemRepo.Insert(pg);
                     _productRepo.Update(p);

                }
            }
            res.IsSuccess = true;
            res.Code = (int)HttpStatusCode.OK;
            res.Data = null;


            return res;
        }
    }
}
