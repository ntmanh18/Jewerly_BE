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
        public Task<IEnumerable<Cashier>> GetAllCashiers();
        public Task<Cashier> UpdateCashier(Cashier cashierUpdate);
        Task<IEnumerable<Cashier>> GetCashierById();
        public Task<Cashier> DeactiveCashier(Cashier cashierDeactie);
        public Task<Cashier> GetCashierByIdCashier(string cashierId);
        Task<IEnumerable<Cashier>> GetCashiersByUserId();
        Task<IEnumerable<Cashier>> GetCashiersByDate();
        public Task<Cashier?> GetCashierByUser(string userId,DateTime startcash);

        public Task<Cashier> UpdateStatusCashier(Cashier updateStatus);
        public Task<Cashier> DeactiveCashier(Cashier cashierDeactie);
        public Task<Cashier> GetCashierByIdCashier(string cashierId);
        Task<IEnumerable<Cashier>> GetCashiersByUserId();
        Task<IEnumerable<Cashier>> GetCashiersByDate();
    }
}
