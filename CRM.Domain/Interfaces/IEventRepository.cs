using CRM.Domain.Entities;

namespace CRM.Domain.Interfaces;

public interface IEventRepository
{
    Task<Event> GetEventByIdAsync(Guid eventId);
    Task<IEnumerable<Event>> GetAllEventsAsync();
    Task AddEventAsync(Event eventEntity);
    Task UpdateEventAsync(Event eventEntity);
    Task DeleteEventAsync(Guid eventId);
}