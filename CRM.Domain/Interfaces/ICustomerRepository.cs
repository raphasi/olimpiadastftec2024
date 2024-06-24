using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> GetCustomerByIdAsync(Guid customerId);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task AddCustomerAsync(Customer customerEntity);
    Task UpdateCustomerAsync(Customer customerEntity);
    Task DeleteCustomerAsync(Guid customerId);
    void DetachCustomerAsync(Customer customerEntity);
    Task<IEnumerable<Customer>> SearchAsync(string query);
    Task<IEnumerable<Customer>> GetTop10Async();
}