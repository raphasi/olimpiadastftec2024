using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _customerContext;

        public CustomerRepository(ApplicationDbContext customerContext)
        {
            _customerContext = customerContext;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            return await _customerContext.Set<Customer>().FindAsync(customerId);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerContext.Set<Customer>().ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customerEntity)
        {
            await _customerContext.Set<Customer>().AddAsync(customerEntity);
            await _customerContext.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customerEntity)
        {
            _customerContext.Set<Customer>().Update(customerEntity);
            await _customerContext.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            var customer = await _customerContext.Set<Customer>().FindAsync(customerId);
            if (customer != null)
            {
                _customerContext.Set<Customer>().Remove(customer);
                await _customerContext.SaveChangesAsync();
            }
        }
    }
}