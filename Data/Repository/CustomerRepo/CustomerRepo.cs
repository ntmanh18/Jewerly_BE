using Data.Entities;
using Data.Repository.GenericRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.CustomerRepo
{
    public class CustomerRepo : Repository<Customer>, ICustomerRepo
    {
        private readonly JewerlyV6Context _context;
        public CustomerRepo(JewerlyV6Context context) : base(context)
        {
            _context = context;
        }
        public async Task CreateCustomer(Customer customerFilter)
        {
            await _context.AddAsync(customerFilter);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }
    }
}
