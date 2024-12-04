using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.ProductBillRepo
{
    public class ProductBillRepo : Repository<ProductBill>, IProductBillRepo
    {
        private readonly JewerlyV6Context _context;
        public ProductBillRepo(JewerlyV6Context context) :base(context) 
        {
            _context = context;
        }

        public async Task<List<ProductBill>> GetProducByBillId(string billId)
        {
            return await _context.ProductBills.Where(x => x.BillBillId == billId).Include(x => x.BillBill ).ThenInclude(x=> x.VoucherVoucher).Include(x=> x.ProductProduct).ToListAsync();
        }

        public async Task<ProductBill> GetUniqueProductBill(string billId, string productId)
        {
            return await _context.ProductBills.Where(x => x.ProductProductId == productId && x.BillBillId == billId).Include(c => c.BillBill).Include(c=> c.ProductProduct).FirstOrDefaultAsync();
        }
    }
}
