using Data.Entities;
using Data.Model.ProductModel;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.ProductRepo
{
    public interface IProductRepo
    public interface IProductRepo : IRepository<Product>    
    {
        public Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductsByName();
        Task<IEnumerable<Product>> GetProductById();
        public Task<Product> UpdateProduct(Product productUpdate);
        Task<List<Product>> GetAllProductsv2();
        Task<Product> GetProductByIdv2(string id);   

        public Task<Gold> GetGoldById(string goldId);


    }
}
