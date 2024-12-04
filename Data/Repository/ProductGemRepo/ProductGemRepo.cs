using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.ProductGemRepo
{
    public class ProductGemRepo : Repository<ProductGem>,   IProductGemRepo
    {
        private readonly JewerlyV6Context _context;
        public ProductGemRepo(JewerlyV6Context context) :base(context) {
        _context = context;
        }

        public async Task<ProductGem> GetProductGemUnique(string productId, string gemId)
        {
            return await _context.ProductGems.FirstOrDefaultAsync(c => c.ProductProductId == productId && c.GemGemId==gemId);
            

        }
    }
}
