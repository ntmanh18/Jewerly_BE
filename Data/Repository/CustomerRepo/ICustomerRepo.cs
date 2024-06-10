using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.CustomerRepo
{
    public interface ICustomerRepo
    {
        public Task CreateCustomer(Customer customerFilter);
        public Task<IEnumerable<Customer>> GetCustomers();
        Task<IEnumerable<Customer>> GetCustomersByName();
        Task<IEnumerable<Customer>> GetCustomerByPhone();
        Task<IEnumerable<Customer>> GetCustomerById();
        public Task<Customer> UpdateCustomer(Customer customerUpdate);
        //public Task<Customer> DeactiveCustomer(Customer customerDeactive);

        public Task<Customer> GetCustomerById(string customerId);
    }
}
