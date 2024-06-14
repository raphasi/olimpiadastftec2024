using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _eventContext;

        public EventRepository(ApplicationDbContext eventContext)
        {
            _eventContext = eventContext;
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            return await _eventContext.Set<Event>().FindAsync(eventId);
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _eventContext.Set<Event>().ToListAsync();
        }

        public async Task AddEventAsync(Event eventEntity)
        {
            await _eventContext.Set<Event>().AddAsync(eventEntity);
            await _eventContext.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event eventEntity)
        {
            _eventContext.Set<Event>().Update(eventEntity);
            await _eventContext.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Guid eventId)
        {
            var eventEntity = await _eventContext.Set<Event>().FindAsync(eventId);
            if (eventEntity != null)
            {
                _eventContext.Set<Event>().Remove(eventEntity);
                await _eventContext.SaveChangesAsync();
            }
        }
    }
}