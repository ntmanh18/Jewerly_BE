using Data.Entities;
using Data.Model.CustomerModel;
using Data.Model.ProductModel;
using Data.Model.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Services.CustomerService
{
    public interface ICustomerService
    {
        public Task<ResultModel> CreateCustomer(string token, CustomerCreateModel customerModel);
        public Task<IEnumerable<CustomerViewModel>> GetCustomers();
        Task<ResultModel> GetCustomersByName(string? token, string name);
        Task<ResultModel> GetCustomerByPhone(string? token, string phone);
        Task<ResultModel> GetCustomerById(string? token, string id);
        Task<ResultModel> UpdateCustomer(string? token, CustomerUpdateModel customerModel);
        Task<ResultModel> DeactiveCustomer(string? token, string id);
        Task<Customer> GetCustomerById(string customerId);
        Task<ResultModel> UpdateStatusCustomer(string? token, string id);


    }
}
