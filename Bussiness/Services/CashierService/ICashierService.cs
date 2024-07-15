using Data.Entities;
using Data.Model.CashierModel;
using Data.Model.CustomerModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.CashierService
{
    public interface ICashierService
    {
        public Task<ResultModel> CreateCashier(string token, CashierRequestModel cashierModel);
        public Task<ResultModel> GetAllCashiers(string token);
        Task<ResultModel> UpdateCashier(string? token, CashierUpdateModel cashierModel);
        Task<ResultModel> DeactiveCashier(string? token, string id);
        Task<ResultModel> GetCashiersByUserId(string? token, string id);
        Task<ResultModel> GetCashiersByDate(string? token, DateTime dateStart, DateTime dateEnd, int num);
        Task<ResultModel> GetIncomeByDate(string? token, DateTime dateStart, DateTime dateEnd, int num);
        Task<User> GetUserById(string userId);
        public Task<List<Cashier>> GetCashiers();
        Task<ResultModel> GetCashierById(string id);
        Task<Cashier> GetCashierByIdCashier(string cashierId);
        Task<ResultModel> UpdateStatusCashier(string? token, string id);
    }
}
