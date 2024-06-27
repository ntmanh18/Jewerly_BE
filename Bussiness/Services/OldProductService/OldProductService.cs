using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.OldProductModel;
using Data.Model.ResultModel;
using Data.Repository.OldProductRepo;
using Microsoft.EntityFrameworkCore;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.OldProductService
{
    public class OldProductService : IOldProductService
    {
        private static Random _random = new Random();
        private readonly IOldProductRepo _OPrepo;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;
        private readonly JewerlyV6Context _context;
        public OldProductService(IOldProductRepo OPrepo, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService, JewerlyV6Context context)
        {
            _OPrepo = OPrepo;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
            _context = context;
        }

        public async Task<ResultModel> GetAllAsync(string token)
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
            var oldProducts = await _OPrepo.GetAllAsync();



            IEnumerable<OldProductRequestModel> list = oldProducts.Select(op => new OldProductRequestModel
            {
                OproductId = op.OproductId,
                ProductProductId = op.ProductProductId,
                Desc = op.Desc,
                BillBillId = op.BillBillId,
            }).ToList();
            resultModel.IsSuccess = true;
            resultModel.Code = 200;
            resultModel.Data = list;
            resultModel.Message = "Correct";
            return resultModel;
        }

        public async Task<ResultModel> GetByIdAsync(string token, string id)
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

            var oldProduct = await _OPrepo.GetByIdAsync(id);
            if (oldProduct == null)
            {
                return null;
            }
            OldProductRequestModel get = new OldProductRequestModel
            {
                OproductId = oldProduct.OproductId,
                ProductProductId = oldProduct.ProductProductId,
                Desc = oldProduct.Desc,
                BillBillId = oldProduct.BillBillId,
            };
            resultModel.IsSuccess = true;
            resultModel.Code = 200;
            resultModel.Data = get;
            resultModel.Message = "Correct";
            return resultModel;

        }

        public async Task<ResultModel> GetByProductIdAsync(string token, string productId)
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
            var oldProducts = await _OPrepo.GetByProductIdAsync(productId);
            IEnumerable < OldProductRequestModel > get = oldProducts.Select(op => new OldProductRequestModel
            {
                OproductId = op.OproductId,
                ProductProductId= op.ProductProductId,
                Desc= op.Desc,
                BillBillId= op.BillBillId,
            }).ToList();
            if (get.Count() == 0)
            {
                resultModel.Message = "Not Found";
            }
            else
            {
                resultModel.Message = "Correct";
            }
            resultModel.IsSuccess = true;
            resultModel.Code = 200;
            resultModel.Data = get;
            
            return resultModel;
        }

        public async Task<ResultModel> AddAsync(string token, OldProductCreateModel oldProduct)
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
            var productExists = await _context.Products.AnyAsync(p => p.ProductId == oldProduct.ProductProductId);
            var billExists = await _context.Bills.AnyAsync(b => b.BillId == oldProduct.BillBillId);

            if (!productExists)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = 400;
                resultModel.Message = "Product not found";
            }
            else if (!billExists)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = 400;
                resultModel.Message = "Bill not found";
            }
            else
            {
                var product = new OldProduct()
                {
                    OproductId = await GenerateOPId(),
                    ProductProductId = oldProduct.ProductProductId,
                    Desc = oldProduct.Desc,
                    BillBillId = oldProduct.BillBillId,

                };
                await _OPrepo.AddAsync(product);
                resultModel.IsSuccess = true;
                resultModel.Code = 400;
                resultModel.Message = "Add success";
                resultModel.Data = oldProduct;
            }
            
            return resultModel;
        }

        //private async Task<string> GenerateOPId()
        //{
        //    string newItem;
        //    var a = await _OPrepo.GetAllAsync();
        //    int randomNumber = _random.Next(0, 100);
        //    string numberPart = randomNumber.ToString("D3");
        //    newItem = "OP" + numberPart;
        //    return newItem; 
        //}
        private async Task<string> GenerateOPId()
        {
            var existingIds = await _OPrepo.GetAllAsync();
            HashSet<string> existingIdSet = new HashSet<string>(existingIds.Select(op => op.OproductId));

            string newItem;
            do
            {
                int randomNumber = _random.Next(1, 100);
                string numberPart = randomNumber.ToString("D3");
                newItem = "OP" + numberPart;
            } while (existingIdSet.Contains(newItem));

            return newItem;
        }
    }
}
