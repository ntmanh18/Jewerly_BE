using Data.Entities;
using Data.Model.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.ProductRepo
{
    public interface IProductRepo
    {
        public Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductsByName();
        Task<IEnumerable<Product>> GetProductById();
        public Task<Product> UpdateProduct(Product productUpdate);
        


    }
}
