 using Bussiness.Services.AccountService;
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
using System;
using System.Net;


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
            IGemRepo gemRepo
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
            Customer customer =await _customerRepo.GetCustomerById(req.CustomerId);
            Voucher voucher = await _voucherRepo.GetVoucherByIdAsync(req.VoucherId);
            Cashier cash = await _cashierRepo.GetCashierByUser(decodeModel.userid, DateTime.Now);
            if(cash == null)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Cash can not be null";
                return res;
            }
            //if voucher belong to customer
           if(voucher.CustomerCustomer != customer)
            {
                res.IsSuccess = false;
                res.Code = (int)HttpStatusCode.Forbidden;
                res.Message = "Invalid voucher";
                return res;
            }
            
            
            foreach(var product in req.Product)
            {
                var existProduct = await _productRepo.GetProductByIdv2(product.Key);
                decimal productPrice = 0;
                decimal gemPrice = GemCost(existProduct.ProductGems.ToList());
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
                productPrice = CalculateCost((decimal)existProduct.MaterialNavigation.SalePrice, (decimal)existProduct.Weight, existProduct.MachiningCost, gemPrice, (decimal)existProduct.MarkupRate);
                if(existProduct.DiscountDiscounts.Count > 0)
                {
                    var disCountList = new List<Discount>();
                    //check available disount
                   foreach(var discount in existProduct.DiscountDiscounts)
                    {
                        if(discount.PublishDay.CompareTo(DateTime.UtcNow) <= 0 &&
                            discount.ExpiredDay.CompareTo(DateTime.UtcNow) >=0 
                            ) {
                            disCountList.Add(discount);
                        }
                    }
                  productPrice = CostWithDiscount(productPrice,disCountList);
                }

                totalCost += productPrice;
            }
            if(voucher != null)
            {
                totalCost = CostWithVoucher(totalCost, voucher.Cost);
            }
            try
            {

                
                //create bill
                Bill b = new Bill();
                b.BillId = GenerateId();
                b.TotalCost = totalCost;
                b.PublishDay = DateTime.Now;
                if (voucher != null)
                {
                    b.VoucherVoucherId = voucher.VoucherId;
                }
                b.CashierId = cash.CashId;
                if(customer != null) { b.CustomerId = customer.CustomerId; }
                
                
                
                await _billRepo.Insert(b);
                // add relationship product bill
                CreateProductBillReqModel model = new CreateProductBillReqModel()
                {
                    BillId = GenerateId(),
                    Product = req.Product,
                };
                await _productBillService.CreateProductBill(token, model);
                //update cash income
                cash.Income += totalCost;
                await _cashierRepo.UpdateCashier(cash);
                //update customer point
                if(customer != null)
                {
                    int point = customer.Point += (int)Math.Floor((totalCost / 100000));
                }
                //delete voucher
                if (voucher != null) {
                    await _voucherRepo.DeleteVoucherAsync(voucher);
                }
                //create warranty
                return res;
            }catch (Exception ex)
            {
                res.Message= ex.ToString();
                res.IsSuccess = false;
                
            }
            return res;
        }
    }
}

