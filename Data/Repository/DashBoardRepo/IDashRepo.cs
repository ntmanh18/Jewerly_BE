using Data.Entities;
using Data.Model.DashBoardModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.DashBoardRepo
{
    public interface IDashRepo
    {
        Task<IEnumerable<IncomeByMonth>> GetIncomeByMonthAsync();
        Task<decimal> GetTotalIncomeAsync();
        Task<IEnumerable<IGrouping<int, Cashier>>> GetIncomeByCashNumberAsync();
    }
}
