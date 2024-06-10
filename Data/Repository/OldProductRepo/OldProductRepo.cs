using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.OldProductRepo
{
    public class OldProductRepo : Repository<OldProduct>, IOldProductRepo
    {
        private readonly JewerlyV6Context _context;

        public OldProductRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OldProduct>> GetAllAsync()
        {
            return await _context.OldProducts
                .Include(op => op.ProductProduct)
                .Include(op => op.BillBill)
                .ToListAsync();
        }

        public async Task<OldProduct> GetByIdAsync(string id)
        {
            return await _context.OldProducts
                .Include(op => op.ProductProduct)
                .Include(op => op.BillBill)
                .FirstOrDefaultAsync(op => op.OproductId == id);
        }

        public async Task<IEnumerable<OldProduct>> GetByProductIdAsync(string productId)
        {
            return await _context.OldProducts
                .Include(op => op.ProductProduct)
                .Include(op => op.BillBill)
                .Where(op => op.ProductProductId == productId)
                .ToListAsync();
        }

        public async Task AddAsync(OldProduct oldProduct)
        {
            await _context.OldProducts.AddAsync(oldProduct);
            await _context.SaveChangesAsync();
        }

    }
}
