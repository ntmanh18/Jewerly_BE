using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.DiscountProductRepo
{
    public class DiscountProductRepo : Repository<DiscountProduct>, IDiscountProductRepo
    {
        private readonly JewerlyV6Context _context;
        public DiscountProductRepo(JewerlyV6Context context) :base(context) 
        {
            _context = context;
        }
        public async Task CreateDiscountProduct(DiscountProduct discountProduct)
        {
           _context.Add(discountProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscountProduct(DiscountProduct discountProduct)
        {
             _context.Remove(discountProduct);
            await _context.SaveChangesAsync();

        }

        public async Task<List<DiscountProduct>> GetAllDiscountProduct()
        {
            return await _context.DiscountProducts.ToListAsync();
        }

        public async Task<List<DiscountProduct>> GetDiscountProductByDiscout(string discountId)
        {
            return await _context.DiscountProducts.Where(x => x.DiscountDiscountId == discountId).Include(x => x.ProductProduct).Include(x => x.DiscountDiscount).Include(x => x.ProductProduct).Include(x => x.DiscountDiscount).ToListAsync();
        }

        public async Task<List<DiscountProduct>> GetDiscountProductByProduct(string productId)
        {
            return await _context.DiscountProducts.Where(x => x.ProductProductId == productId). Include(x => x.ProductProduct).Include(x=> x.DiscountDiscount).ToListAsync();
        }

        public async Task<DiscountProduct> GetUniqueDiscountProduct(string discountId, string productId)
        {
            return await _context.DiscountProducts.Where(x => x.ProductProductId == productId && x.DiscountDiscountId == discountId).Include(x => x.ProductProduct).Include(x => x.DiscountDiscount).FirstOrDefaultAsync();
        }
    }
}
