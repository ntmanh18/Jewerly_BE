using Bussiness.Services.AccountService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.ProductBillModel;
using Data.Model.ResultModel;
using Data.Repository.BillRepo;
using Data.Repository.GemRepo;
using Data.Repository.ProductBillRepo;
using Data.Repository.ProductGemRepo;
using Data.Repository.ProductRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.ProductBillService
{
    public class ProductBillService : IProductBillService
    {
        private readonly IProductBillRepo _producBillRepo;
        private readonly IProductRepo _productRepo;
        private readonly IToken _token;
        private readonly IAccountService _accountService;
        private readonly IBillRepo _billRepo;

        public ProductBillService(IProductBillRepo productBillRepo,
            IProductRepo productRepo,
            IToken token,
            IAccountService accountService,
            IBillRepo billRepo
            )
        {
            _accountService = accountService;
            _producBillRepo = productBillRepo;  
            _productRepo = productRepo;
            _token = token;
            _billRepo = billRepo;
        }
        public async Task<ResultModel> CreateProductBill(string token, CreateProductBillReqModel req)

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
            List<ProductBill> pbList = new List<ProductBill>();

            if (!isValidRole)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";

                return res;
            }
            Bill b = await _billRepo.Get(req.BillId);

            if (b == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Bill is not existed";
                return res;
            }

            foreach (var id in req.Product)
            {
                Product p = await _productRepo.GetProductByIdv2(id.Key);
                if (p == null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Product is not existed";
                    return res;
                }
                if (!isValidAmount(id.Value, p.Amount))
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Invalid Amount";
                    return res;
                }

                else if (await _producBillRepo.GetUniqueProductBill(b.BillId,id.Key) != null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Product already Existed";
                    return res;
                }
                else
                {
                    ProductBill pb = new ProductBill()
                    {
                      BillBillId = b.BillId,
                      ProductProductId = id.Key,
                      Amount = id.Value 
                    };
                    pbList.Add(pb);
                    await _producBillRepo.Insert(pb);
                    var updateProduct = await _productRepo.GetProductByIdv2(id.Key);
                    updateProduct.Amount = updateProduct.Amount - id.Value;
                    await _productRepo.UpdateProduct(updateProduct);

                    
                   
                }
            }
            res.IsSuccess = true;
            res.Code = (int)HttpStatusCode.OK;
            res.Data = pbList;
            res.Message = "Create Product Bill Successfully";
            return res;
        }
        private static bool isValidAmount(int amount, int validAmount)
        {
            return amount > 0 && amount <= validAmount;
    

    }
    }
    
}

