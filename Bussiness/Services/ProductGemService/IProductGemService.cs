using Data.Entities;
using Data.Model.ProductGemModel;
using Data.Model.ProductModel;
using Data.Model.ResultModel;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.ProductGemService
{
    public interface IProductGemService 
    {
        Task<ResultModel> CreateProductGem(string token,ProductGemReqModel req);
    }
}
