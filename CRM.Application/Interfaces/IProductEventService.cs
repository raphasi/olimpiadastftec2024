using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface IProductEventService
    {
        Task<ProductEventDTO> GetByIdAsync(Guid productEventId);
        Task<IEnumerable<ProductEventDTO>> GetAllAsync();
        Task<ProductEventDTO> AddAsync(ProductEventDTO createProductEventDto);
        Task UpdateAsync(ProductEventDTO updateProductEventDto);
        Task DeleteAsync(Guid productEventId);
        Task<IEnumerable<ProductDTO>> GetProductsByEventIdAsync(Guid eventId);
        Task<IEnumerable<EventDTO>> GetEventsByProductIdAsync(Guid productId);
    }
}