using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Application.DTOs;

namespace CRM.Application.Interfaces
{
    public interface ICartItemService
    {
        Task<CartItemDTO> GetByIdAsync(Guid cartItemId);
        Task<IEnumerable<CartItemDTO>> GetAllByCartIdAsync(Guid cartId);
        Task<CartItemDTO> AddAsync(CartItemDTO cartItemDto);
        Task UpdateAsync(CartItemDTO cartItemDto);
        Task DeleteAsync(Guid cartItemId);
    }
}