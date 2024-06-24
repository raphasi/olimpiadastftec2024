using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class ProductEventRepository : IProductEventRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductEventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductEvent> GetByIdAsync(Guid productEventId)
        {
            return await _context.ProductEvents
                .Include(pe => pe.Product)
                .Include(pe => pe.Event)
                .FirstOrDefaultAsync(pe => pe.Id == productEventId);
        }

        public async Task<IEnumerable<ProductEvent>> GetAllAsync()
        {
            return await _context.ProductEvents
                .Include(pe => pe.Product)
                .Include(pe => pe.Event)
                .ToListAsync();
        }

        public async Task AddAsync(ProductEvent productEvent)
        {
            await _context.ProductEvents.AddAsync(productEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductEvent productEvent)
        {
            _context.ProductEvents.Update(productEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid productEventId)
        {
            var productEvent = await GetByIdAsync(productEventId);
            if (productEvent != null)
            {
                _context.ProductEvents.Remove(productEvent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductEvent>> GetByEventIdAsync(Guid eventId)
        {
            return await _context.ProductEvents
                .Include(pe => pe.Product)
                .Where(pe => pe.EventID == eventId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductEvent>> GetByProductIdAsync(Guid productId)
        {
            return await _context.ProductEvents
                .Include(pe => pe.Event)
                .Where(pe => pe.ProductID == productId)
                .ToListAsync();
        }
    }
}