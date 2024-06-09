using Data.Model.DiscountModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.DiscountService
{
    public interface IDiscountService
    {
        Task<ResultModel> GetAllDiscount(string token, DiscountQueryModel query);
        Task<ResultModel> CreateDiscount(string token, CreateDiscountReqModel req);
        Task<ResultModel> CreateDiscountProduct(string token,CreateDiscountProductReqModel req);
        
    }
}
