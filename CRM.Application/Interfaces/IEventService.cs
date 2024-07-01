using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventDTO>> GetAllAsync();
    Task<EventDTO> GetByIdAsync(Guid id);
    Task<EventDTO> AddAsync(EventDTO evento);
    Task UpdateAsync(EventDTO evento);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<EventDTO>> SearchAsync(string query);
}
