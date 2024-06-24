using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetByIdAsync(Guid cartId);
        Task<IEnumerable<Cart>> GetAllAsync();
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Guid cartId);
    }
}