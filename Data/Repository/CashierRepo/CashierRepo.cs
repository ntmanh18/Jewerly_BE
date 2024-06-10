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

        public async Task<IEnumerable<Cashier>> GetAllCashiers()
        {
            var cashiers = await _context.Cashiers.ToListAsync();
            return cashiers;
        }

        public async Task<Cashier> UpdateCashier(Cashier cashierUpdate)
        {
            try
            {
                _context.Update(cashierUpdate);
                await _context.SaveChangesAsync();
                return cashierUpdate;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<Cashier>> GetCashierById()
        {
            return await _context.Cashiers.ToListAsync();
        }

        public async Task<Cashier?> GetCashierByUser(string userId, DateTime cash)
        {
            return await _context.Cashiers.Where(c => c.UserId == userId && c.StartCash <= cash && c.EndCash >= cash).FirstOrDefaultAsync();}
        public async Task<Cashier> DeactiveCashier(Cashier cashierDeavtive)
        {
            try
            {
                _context.Update(cashierDeavtive);
                await _context.SaveChangesAsync();
                return cashierDeavtive;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<Cashier> GetCashierByIdCashier(string cashierId)
        {
            return await _context.Cashiers.FirstOrDefaultAsync(c => c.CashId == cashierId);
        }

        public async Task<IEnumerable<Cashier>> GetCashiersByUserId()
        {
            return await _context.Cashiers.ToListAsync();
        }
        public async Task<IEnumerable<Cashier>> GetCashiersByDate()
        {
            return await _context.Cashiers.ToListAsync();
        }
        public async Task<Cashier> UpdateStatusCashier(Cashier cashierUpdate)
        {
            try
            {
                _context.Update(cashierUpdate);
                await _context.SaveChangesAsync();
                return cashierUpdate;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<Cashier> GetCashierByIdCashier(string cashierId)
        {
            return await _context.Cashiers.FirstOrDefaultAsync(c => c.CashId == cashierId);
        }

        public async Task<IEnumerable<Cashier>> GetCashiersByUserId()
        {
            return await _context.Cashiers.ToListAsync();
        }
        public async Task<IEnumerable<Cashier>> GetCashiersByDate()
        {
            return await _context.Cashiers.ToListAsync();
        }
        
    }
}
