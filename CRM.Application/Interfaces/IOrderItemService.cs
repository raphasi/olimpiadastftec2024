using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IOrderItemService
{
    Task<IEnumerable<OrderItemDTO>> GetAllAsync();
    Task<OrderItemDTO> GetByIdAsync(Guid id);
    Task<OrderItemDTO>AddAsync(OrderItemDTO orderItem);
    Task UpdateAsync(OrderItemDTO orderItem);
    Task DeleteAsync(Guid id);
}
