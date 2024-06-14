using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _orderItemContext;

        public OrderItemRepository(ApplicationDbContext orderItemContext)
        {
            _orderItemContext = orderItemContext;
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(Guid orderItemId)
        {
            return await _orderItemContext.Set<OrderItem>().FindAsync(orderItemId);
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _orderItemContext.Set<OrderItem>().ToListAsync();
        }

        public async Task AddOrderItemAsync(OrderItem orderItemEntity)
        {
            await _orderItemContext.Set<OrderItem>().AddAsync(orderItemEntity);
            await _orderItemContext.SaveChangesAsync();
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItemEntity)
        {
            _orderItemContext.Set<OrderItem>().Update(orderItemEntity);
            await _orderItemContext.SaveChangesAsync();
        }

        public async Task DeleteOrderItemAsync(Guid orderItemId)
        {
            var orderItem = await _orderItemContext.Set<OrderItem>().FindAsync(orderItemId);
            if (orderItem != null)
            {
                _orderItemContext.Set<OrderItem>().Remove(orderItem);
                await _orderItemContext.SaveChangesAsync();
            }
        }
    }
}