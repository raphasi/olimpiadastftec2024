using CRM.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IActivityService
{
    Task<IEnumerable<ActivityDTO>> GetAllAsync();
    Task<ActivityDTO> GetByIdAsync(Guid id);
    Task<ActivityDTO> AddAsync(ActivityDTO activity);
    Task UpdateAsync(ActivityDTO activity);
    Task DeleteAsync(Guid id);
}
