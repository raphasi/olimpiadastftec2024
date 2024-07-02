using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IOpportunityService
{
    Task<IEnumerable<OpportunityDTO>> GetAllAsync();
    Task<OpportunityDTO> GetByIdAsync(Guid id);
    Task<OpportunityDTO> AddAsync(OpportunityDTO opportunity);
    Task UpdateAsync(OpportunityDTO opportunity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<OpportunityDTO>> SearchAsync(string query);
    Task<(int Count, decimal? EstimatedValue)> GetCountAndEstimatedValueAsync();
}
