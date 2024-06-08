using Data.Entities;
using Data.Model.ProductGemModel;
using Data.Model.ProductModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.ProductService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductRequestModel>> GetProducts();
        Task<ResultModel> GetProductsByName(string? token, string name);
        Task<ResultModel> GetProductById(string? token, string productId);
        Task<ResultModel> UpdateProduct(string? token, ProductRequestModel productModel);
        Task<ResultModel> CreateProduct(string token, CreateProductReqModel productModel);
    }
}
