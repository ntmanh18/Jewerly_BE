using Data.Entities;
using Data.Model.DashBoardModel;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.DashBoardRepo
{
    public class DashRepo : Repository<Cashier>, IDashRepo
    {
        private readonly JewerlyV6Context _context;
        public DashRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }
        public async Task<decimal> GetTotalIncomeAsync()
        {
            //return await _context.Cashiers.SumAsync(c => c.Income ?? 0);
            return await _context.Cashiers.SumAsync(c => c.Income );
        }

        public async Task<IEnumerable<IGrouping<int, Cashier>>> GetIncomeByCashNumberAsync()
        {
            return await _context.Cashiers.GroupBy(c => c.CashNumber).ToListAsync();
        }
        public async Task<IEnumerable<IncomeByMonth>> GetIncomeByMonthAsync()
        {
            var result = await _context.Cashiers
                .GroupBy(c => new { c.StartCash.Year, c.StartCash.Month })
                .Select(g => new IncomeByMonth
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    //TotalIncome = g.Sum(c => c.Income ?? 0)
                    TotalIncome = g.Sum(c => c.Income)

                })
                .Where(dto => dto.TotalIncome != null)
                .ToListAsync();

            return result;
        }
    }
}
