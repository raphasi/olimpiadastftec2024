using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _orderContext;

        public OrderRepository(ApplicationDbContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderContext.Set<Order>().FindAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderContext.Set<Order>().ToListAsync();
        }

        public async Task AddOrderAsync(Order orderEntity)
        {
            await _orderContext.Set<Order>().AddAsync(orderEntity);
            await _orderContext.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order orderEntity)
        {
            _orderContext.Set<Order>().Update(orderEntity);
            await _orderContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _orderContext.Set<Order>().FindAsync(orderId);
            if (order != null)
            {
                _orderContext.Set<Order>().Remove(order);
                await _orderContext.SaveChangesAsync();
            }
        }
    }
}