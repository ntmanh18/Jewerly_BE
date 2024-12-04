using Data.Entities;
using Data.Model.DiscountModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.DiscountProductService
{
    public interface IDiscountProductService
    {
        Task<ResultModel> CreateDiscountProduct(string token, CreateDiscountProductReqModel req);
        Task<ResultModel> DeleteDiscountProduct(string token, CreateDiscountProductReqModel req);


    }
}
