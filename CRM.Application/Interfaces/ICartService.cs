using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Application.DTOs;

namespace CRM.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetByIdAsync(Guid cartId);
        Task<IEnumerable<CartDto>> GetAllAsync();
        Task<CartDto> AddAsync(CartDto cartDto);
        Task UpdateAsync(CartDto cartDto);
        Task DeleteAsync(Guid cartId);
    }
}