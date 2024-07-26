using Data.Entities;
using Data.Repository.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.ProductGemRepo
{
    public interface IProductGemRepo :IRepository<ProductGem>
    {
        Task<ProductGem> GetProductGemUnique(string productId, string gemId);
        Task<List<ProductGem>> GetByProduct(string productId);

    }
}
