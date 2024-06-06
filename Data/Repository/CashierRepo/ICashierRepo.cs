using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.CashierRepo
{
    public interface ICashierRepo
    {
        public Task CreateCashier(Cashier cashierFilter);
        public Task<User> GetUserById(string userId);

        public Task<List<Cashier>> GetCashiers();
    }
}
