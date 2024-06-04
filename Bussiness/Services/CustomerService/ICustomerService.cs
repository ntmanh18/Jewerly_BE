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
    }
}
