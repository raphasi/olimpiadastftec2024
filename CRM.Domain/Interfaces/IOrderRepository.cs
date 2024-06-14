using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task AddOrderAsync(Order orderEntity);
    Task UpdateOrderAsync(Order orderEntity);
    Task DeleteOrderAsync(Guid orderId);
}