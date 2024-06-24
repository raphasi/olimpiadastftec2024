using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetByIdAsync(Guid cartItemId);
        Task<IEnumerable<CartItem>> GetAllByCartIdAsync(Guid cartId);
        Task AddAsync(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(Guid cartItemId);
    }
}