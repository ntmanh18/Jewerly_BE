using Data.Entities;
using Data.Model.ProductModel;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.ProductRepo
{
    public class ProductRepo : Repository<Product>, IProductRepo
    {
        private readonly JewerlyV6Context _context;
        public ProductRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return products;

        }
        public async Task<IEnumerable<Product>> GetProductsByName()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductById()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product productUpdate)
        {
            try
            {
                _context.Update(productUpdate);
                await _context.SaveChangesAsync();
                return productUpdate;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);


            }


        }
    }
}
