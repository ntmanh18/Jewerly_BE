
﻿ using Bussiness.Services.AccountService;
using Data.Model.ProductBillModel;
using Bussiness.Services.ProductBillService;
using Bussiness.Services.TokenService;
using Data.Entities;
using Data.Model.BillModel;
using Data.Model.ResultModel;
using Data.Repository.BillRepo;
using Data.Repository.CashierRepo;
using Data.Repository.CustomerRepo;
using Data.Repository.DiscountRepo;
using Data.Repository.GemRepo;
using Data.Repository.ProductBillRepo;
using Data.Repository.ProductRepo;
using Data.Repository.VoucherRepo;
using System.Net;
using Data.Model.VoucherModel;
using Microsoft.EntityFrameworkCore;
using Data.Model.WarrantyModel;
using Bussiness.Services.WarrantyService;



namespace Bussiness.Services.BillService
{
    public class BillService : IBillService
    {
        static Random _random = new Random();

        private readonly IProductBillRepo _producBillRepo;
        private readonly IProductRepo _productRepo;
        private readonly IToken _token;
        private readonly IAccountService _accountService;
        private readonly IBillRepo _billRepo;

        private readonly ICustomerRepo _customerRepo;
        private readonly IVoucherRepo _voucherRepo;
        private readonly IDiscountRepo _discountRepo;
        private readonly ICashierRepo _cashierRepo;
        private readonly IProductBillService _productBillService;
        private readonly IGemRepo _gemRepo;
        private readonly IWarrantyService _warrantyService;

        public BillService(IProductBillRepo productBillRepo,
            IProductRepo productRepo,
            IToken token,
            IAccountService accountService,
            IBillRepo billRepo,
            ICustomerRepo customerRepo,
            IVoucherRepo voucherRepo,
            IDiscountRepo discountRepo,
            ICashierRepo cashierRepo,
            IProductBillService productBillService,
            IGemRepo gemRepo,
            IWarrantyService warrantyService

            )
        {
            _accountService = accountService;
            _producBillRepo = productBillRepo;
            _productRepo = productRepo;
            _token = token;
            _billRepo = billRepo;

            _customerRepo = customerRepo;
            _voucherRepo = voucherRepo; 
            _discountRepo = discountRepo;   
            _cashierRepo = cashierRepo; 
            _productBillService = productBillService;
            _gemRepo = gemRepo; 
            _warrantyService = warrantyService;

        }

        private  string GenerateId()
        {
            int randomNumber = _random.Next(0, 10000);
            var bill = _billRepo.GetAll();
            var length = bill.Result.Count() + 1;
            return  "B" + randomNumber.ToString() +  length.ToString();
        }


        private decimal CalculateCost(decimal gold, decimal weight, decimal material, decimal gem, decimal markup )
        {
            decimal cost = 0;
             cost = (decimal)(((gold * weight) + material + gem) * markup);
            return cost;
        }
        private decimal CostWithVoucher(decimal totalBill, decimal voucher)
        {
            return totalBill - (totalBill*voucher);
        }
        private decimal CostWithDiscount(decimal productCost, List<Discount> discountList)
        {
            if(discountList.Count <= 0) { return productCost; }
            decimal cost = productCost;
            foreach(Discount discount in discountList)
            {
                cost = cost - discount.Cost;
            }
            return cost;
        }
        private static bool isValidAmount(int amount, int validAmount)
        {
            return amount > 0 && amount <= validAmount;


        }
        private long GemCost(List<ProductGem> gems)
        {
            long gemCost = 0;
            foreach (ProductGem pGem in gems)
            {
                gemCost += pGem.GemGem.Price;
            }
            return gemCost;
        }

        public async Task<ResultModel> CraeteBill(string token, CreateBillReqModel req)
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
           
            List<ProductBill> pbList = new List<ProductBill>();

            if (!isValidRole)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "You don't permission to perform this action.";
                return res;
            }
            decimal totalCost = 0;
            var customer = await _customerRepo.GetCustomerById(req.CustomerId);
            var voucher =await _voucherRepo.GetVoucherByIdAsync(req.VoucherId);
            var cash =await  _cashierRepo.GetCashierByUser(decodeModel.userid, DateTime.Now);
            
            if (cash == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Cash can not be null";
                return res;
            }
            //if voucher belong to customer
           
            
            
            foreach(var product in req.Product)
            {
                var existProduct = await _productRepo.GetProductByIdv2(product.Key);
                decimal productPrice = existProduct.Price;
                decimal gemPrice = 0;
                

                if (existProduct == null)
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Product is not existed";
                    return res;
                }
                if (!isValidAmount(product.Value, existProduct.Amount))
                {
                    res.IsSuccess = false;
                    res.Code = (int)HttpStatusCode.Forbidden;
                    res.Message = "Invalid Amount";
                    return res;
                }
                //else if (req.Product.ContainsKey(product.Key))
                //{
                //    res.IsSuccess = false;
                //    res.Code = (int)HttpStatusCode.Forbidden;
                //    res.Message = "Product already Existed";
                //    return res;
                //}
                
                if(existProduct.DiscountProducts.Count > 0)
                {
                    var disCountList = new List<Discount>();
                    //check available disount
                   foreach(var discount in existProduct.DiscountProducts)
                    {
                       
                          var checkDiscount = await _discountRepo.GetDiscountById(discount.DiscountDiscountId);
                            
                        

                            if (checkDiscount.PublishDay.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) <= 0 &&
                            checkDiscount.ExpiredDay.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) >=0 
                            ) {
                            disCountList.Add(checkDiscount);
                        }
                    }
                  productPrice = CostWithDiscount(existProduct.Price , disCountList);
                }

                totalCost = totalCost +( productPrice * product.Value);
            }

            if(voucher != null)
            {
               
                totalCost = CostWithVoucher(totalCost, voucher.Cost);
            }
            try
            {
                //create bill
                Bill b = new Bill();
                b.Type = true;
                b.BillId = GenerateId();
                b.TotalCost = totalCost;
                b.PublishDay = DateTime.Now;
                b.Payment = req.PaymentType;
                
                
                if (voucher != null)
                {
                    b.VoucherVoucherId = voucher.VoucherId;
                }
                b.CashierId = cash.CashId;
                if(customer != null) { b.CustomerId = customer.CustomerId; }


                b.BillId = GenerateId();
                List<ProductBill> pb = new List<ProductBill>();
                
                foreach (var p in req.Product)
                {
                    if (customer != null)
                    {
                        WarrantyCreateModel warranty = new WarrantyCreateModel()
                        {
                            CustomerId = customer.CustomerId,
                            StartDate = DateTime.Now,
                            ProductId = p.Key,
                            
                            Desc = "One-year warranty"
                        };
                      await  _warrantyService.CreateWarranty(token, warranty);
                    }
                    var existProduct = await _productRepo.GetProductByIdv2(p.Key);
                    decimal unitprice = 0;
                    if (existProduct.DiscountProducts.Count > 0)
                    {
                        var disCountList = new List<Discount>();
                        //check available disount
                        foreach (var discount in existProduct.DiscountProducts)
                        {

                            var checkDiscount = await _discountRepo.GetDiscountById(discount.DiscountDiscountId);



                            if (checkDiscount.PublishDay.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) <= 0 &&
                            checkDiscount.ExpiredDay.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) >= 0
                            )
                            {
                                disCountList.Add(checkDiscount);
                            }
                        }
                        unitprice = CostWithDiscount(existProduct.Price, disCountList);
                    }
                    ProductBill item = new ProductBill()
                    {
                        BillBillId = b.BillId,
                        ProductProductId = p.Key,
                        Amount = p.Value,
                        UnitPrice = unitprice,
                        
                    };
                    pb.Add(item);
                }
                if (customer != null)
                {
                    b.CustomerId = customer.CustomerId;

                }
                b.ProductBills = pb;
                 await _billRepo.Insert(b);
                // add relationship product bill
                //CreateProductBillReqModel model = new CreateProductBillReqModel()
                //{
                //    BillId = b.BillId,
                //    Product = req.Product,
                //};
                //await _productBillService.CreateProductBill(token, model);
                //update cash income
                cash.Income += totalCost;
                await _cashierRepo.UpdateCashier(cash);
                //update customer point
                if(customer != null)
                {
                    int point = customer.Point += (int)Math.Floor((totalCost / 100000));
                 await    _customerRepo.UpdateCustomer(customer);

                }
                //delete voucher
                if (voucher != null) {
                    voucher.Status = false;
                    await _voucherRepo.Update(voucher);
                }
                //create warranty
                
                res.Data = b;
                return res;
            }catch (Exception ex)
            {
                res.Message= ex.ToString();
                res.IsSuccess = false;
                
            }
            return res;

    }
        public async Task<ResultModel> ViewBill(string? token, BillSearchModel billSearch)
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
            var query = _billRepo.GetBillQuery();

            if (billSearch.stardate.HasValue || billSearch.enddate.HasValue)
            {
                query = query.Where(v =>
                    (!billSearch.stardate.HasValue || v.PublishDay >= billSearch.stardate.Value) &&
                    (!billSearch.enddate.HasValue || v.PublishDay <= billSearch.enddate.Value)
                );
            }

            if (billSearch.CashNumber.HasValue)
            {
                query = query.Where(v => v.Cashier.CashNumber == billSearch.CashNumber);
            }
            if (billSearch.SortByTotalCost)
            {
                if (billSearch.SortByTotalCostDesc)
                {
                    query = query.OrderByDescending(v => v.TotalCost);
                }
                else
                {
                    query = query.OrderBy(v => v.TotalCost);
                }
            }
            else
            {
                query = query.OrderBy(v => v.PublishDay);
            }
            var Bill = await query.ToListAsync();
            resultModel.Data = Bill;

            return resultModel;
        }
        public async Task<ResultModel> BillCount(string? token)
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
            try
            {
                var totalBill = await _billRepo.TotalBill();
                resultModel.Data = totalBill;
                resultModel.Message = "Total bill retrieved successfully.";
            }
            catch (Exception ex)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.InternalServerError;
                resultModel.Message = ex.Message;
            }

            return resultModel;
        }

        public async Task<ResultModel> GetBillByCash(string token)
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
            Cashier cash = await _cashierRepo.GetCashierByUser(decodeModel.userid, DateTime.Now);
            if (cash == null)
            {
                resultModel.IsSuccess = false;
                resultModel.Code = (int)HttpStatusCode.Forbidden;
                resultModel.Message = "Invalid cash";
                return resultModel;
            }

            List<Bill> bills = await _billRepo.GetBillByCash(cash.CashId);
            resultModel.IsSuccess = true;
            resultModel.Code = (int)HttpStatusCode.OK;
            resultModel.Data = bills;
            return resultModel;

        }

        public async Task<ResultModel> getBillById(string? token, string id)
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
            var query = _billRepo.GetBillById(id);


          

            resultModel.Data = query;

            return resultModel;
        }
    }
}
