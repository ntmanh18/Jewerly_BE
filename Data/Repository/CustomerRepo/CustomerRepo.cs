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

        public async Task<IEnumerable<Customer>> GetCustomersByName()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<IEnumerable<Customer>> GetCustomerByPhone()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomerById()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<Customer> UpdateCustomer(Customer customerUpdate)
        {
            try
            {
                _context.Update(customerUpdate);
                await _context.SaveChangesAsync();
                return customerUpdate;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<Customer> GetCustomerById(string customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        //public async Task<Customer> DeactiveCustomer(Customer customerDeactive)
        //{
        //    try
        //    {
        //        _context.Update(customerDeactive);
        //        await _context.SaveChangesAsync();
        //        return customerDeactive;
        //    }
        //    catch (Exception ex)
        //    {
        public async Task<Customer> DeactiveCustomer(Customer customerDeactive)
        {
            try
            {
                _context.Update(customerDeactive);
                await _context.SaveChangesAsync();
                return customerDeactive;
            }
            catch (Exception ex)
            {

        //        throw new Exception(ex.Message);
        //    }
                throw new Exception(ex.Message);
            }

        //}
        //public async Task<Customer> GetCustomerById(string customerId)
        //{
        //    return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        //}
        }
        public async Task<Customer> GetCustomerById(string customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }
}
