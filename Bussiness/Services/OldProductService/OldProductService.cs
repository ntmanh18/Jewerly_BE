using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.OldProductModel;
using Data.Model.ResultModel;
using Data.Repository.OldProductRepo;
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
        private readonly IOldProductRepo _repository;
        private readonly IAccountService _accountService;
        private readonly IAuthenticateService _authentocateService;
        private readonly IToken _token;
        public OldProductService(IOldProductRepo repository, IToken token,
            IAuthenticateService authenticateService,
            IAccountService accountService)
        {
            _repository = repository;
            _token = token;
            _authentocateService = authenticateService;
            _accountService = accountService;
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
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var oldProducts = await _repository.GetAllAsync();



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
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }

            var oldProduct = await _repository.GetByIdAsync(id);
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
            var isValidRole = _accountService.IsValidRole(decodeModel.role, new List<int>() { 1, 2, 3 });
            if (!isValidRole)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "You don't permission to perform this action.";

                return resultModel;
            }
            var oldProducts = await _repository.GetByProductIdAsync(productId);
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

    }
}
