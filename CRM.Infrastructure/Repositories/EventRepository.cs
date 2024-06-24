using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events
                .Include(e => e.ProductEvents)
                .ThenInclude(pe => pe.Product)
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(Guid id)
        {
            return await _context.Events
                .Include(e => e.ProductEvents)
                .ThenInclude(pe => pe.Product)
                .FirstOrDefaultAsync(e => e.EventID == id);
        }

        public async Task AddEventAsync(Event eventEntity)
        {
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event eventEntity)
        {
            _context.Events.Update(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Guid id)
        {
            var eventEntity = await _context.Events.FindAsync(id);
            if (eventEntity != null)
            {
                _context.Events.Remove(eventEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Event>> GetTop10Async()
        {
            return await _context.Events
                .Include(e => e.ProductEvents)
                .ThenInclude(pe => pe.Product)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> SearchAsync(string query)
        {
            return await _context.Events
                .Include(e => e.ProductEvents)
                .ThenInclude(pe => pe.Product)
                .Where(e => e.Name.Contains(query))
                .ToListAsync();
        }
    }
}