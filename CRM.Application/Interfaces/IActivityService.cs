using CRM.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IActivityService
{
    Task<IEnumerable<ActivityDTO>> GetAllAsync();
    Task<ActivityDTO> GetByIdAsync(Guid id);
    Task AddAsync(ActivityDTO activity);
    Task UpdateAsync(ActivityDTO activity);
    Task DeleteAsync(Guid id);
}
