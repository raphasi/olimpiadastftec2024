using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface ILeadRepository
{
    Task<Lead> GetLeadByIdAsync(Guid leadId);
    Task<IEnumerable<Lead>> GetAllLeadsAsync();
    Task AddLeadAsync(Lead leadEntity);
    Task UpdateLeadAsync(Lead leadEntity);
    Task DeleteLeadAsync(Guid leadId);
    Task<IEnumerable<Lead>> SearchAsync(string query);
    Task<IEnumerable<Lead>> GetTop10Async();
}