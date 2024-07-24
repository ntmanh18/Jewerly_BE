using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.BillRepo
{
    public class BillRepo : Repository<Bill>, IBillRepo
    {
        private readonly JewerlyV6Context _context;
        public BillRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }

        public Task<List<Bill>> GetBillByCash(string cashId)
        {
            return _context.Bills.Where(x => x.CashierId == cashId).OrderByDescending(x => x.PublishDay).ToListAsync();
        }

        public Task<Bill> GetBillById(string billId)
        {
            return _context.Bills.Where(x => x.BillId == billId).Include(x=> x.Customer). FirstOrDefaultAsync();
        }

        public IQueryable<Bill> GetBillQuery() => _context.Bills
            .Include(v => v.Cashier)
            .Include(v => v.Customer)
            .Include(v => v.VoucherVoucher).AsQueryable();
        public async Task<decimal> TotalBill()
        {
            return await _context.Bills.CountAsync();
        }
    }
}
