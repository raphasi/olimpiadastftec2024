using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllAsync();
        Task<CustomerDTO> GetByIdAsync(Guid id);
        Task<CustomerDTO> AddAsync(CustomerDTO customer);
        Task UpdateAsync(CustomerDTO customer);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<CustomerDTO>> SearchAsync(string query);
    }
}
