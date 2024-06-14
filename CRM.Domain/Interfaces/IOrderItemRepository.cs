using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderItem> GetOrderItemByIdAsync(Guid orderItemId);
    Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
    Task AddOrderItemAsync(OrderItem orderItemEntity);
    Task UpdateOrderItemAsync(OrderItem orderItemEntity);
    Task DeleteOrderItemAsync(Guid orderItemId);
}