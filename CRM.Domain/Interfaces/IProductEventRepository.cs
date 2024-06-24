using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IProductEventRepository
{
    Task<ProductEvent> GetByIdAsync(Guid productEventId);
    Task<IEnumerable<ProductEvent>> GetAllAsync();
    Task AddAsync(ProductEvent productEvent);
    Task UpdateAsync(ProductEvent productEvent);
    Task DeleteAsync(Guid productEventId);
    Task<IEnumerable<ProductEvent>> GetByEventIdAsync(Guid eventId);
    Task<IEnumerable<ProductEvent>> GetByProductIdAsync(Guid productId);

}