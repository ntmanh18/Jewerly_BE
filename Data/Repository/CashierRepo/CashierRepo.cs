using Data.Entities;
using Data.Repository.GenericRepo;
using Data.Repository.ProductRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.CashierRepo
{
    public class CashierRepo : Repository<Cashier>, ICashierRepo
    {
        private readonly JewerlyV6Context _context;
        public CashierRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }

        public async Task CreateCashier(Cashier cashierFilter)
        {
            await _context.AddAsync(cashierFilter);
            _context.SaveChanges();
        }
        public async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<Cashier>> GetCashiers()
        {
            var cashiers = await _context.Cashiers.ToListAsync();
            return cashiers;
        }
    }
}
