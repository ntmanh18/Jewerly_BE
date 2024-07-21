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

        public Task<Bill> GetBillByCash(string cashId)
        {
            throw new NotImplementedException();
        }

        public Task<Bill> GetBillById(string billId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Bill> GetBillQuery() => _context.Bills.Include(v => v.Cashier).Include(v => v.Customer).Include(v => v.VoucherVoucher).AsQueryable();
        public async Task<decimal> TotalBill()
        {
            return await _context.Bills.CountAsync();
        }
    }
}
