using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDTO>> GetAllAsync();
    Task<OrderDTO> GetByIdAsync(Guid id);
    Task<OrderDTO> AddAsync(OrderDTO order);
    Task UpdateAsync(OrderDTO order);
    Task DeleteAsync(Guid id);
}
