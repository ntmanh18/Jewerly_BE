using Data.Model.ProductBillModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.ProductBillService
{
    public interface IProductBillService
    {
        Task<ResultModel> CreateProductBill(string token, CreateProductBillReqModel req);
    }
}
