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

    }
}
