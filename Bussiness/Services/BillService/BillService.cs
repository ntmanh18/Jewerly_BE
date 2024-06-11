using Bussiness.Services.AccountService;
using Bussiness.Services.TokenService;
using Data.Model.BillModel;
using Data.Model.ResultModel;
using Data.Repository.BillRepo;
using Data.Repository.ProductBillRepo;
using Data.Repository.ProductRepo;
using System;


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


        public BillService(IProductBillRepo productBillRepo,
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

        private  string GenerateId()
        {
            int randomNumber = _random.Next(0, 10000);
            var bill = _billRepo.GetAll();
            var length = bill.Result.Count() + 1;
            return  "B" + randomNumber.ToString() +  length.ToString();
        }

        public Task<ResultModel> CraeteBill(string token, CreateBillReqModel model)
        {
            throw new NotImplementedException();
        }
    }
}

