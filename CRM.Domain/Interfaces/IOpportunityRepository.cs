using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IOpportunityRepository
{
    Task<Opportunity> GetOpportunityByIdAsync(Guid opportunityId);
    Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync();
    Task AddOpportunityAsync(Opportunity opportunityEntity);
    Task UpdateOpportunityAsync(Opportunity opportunityEntity);
    Task DeleteOpportunityAsync(Guid opportunityId);
    Task<IEnumerable<Opportunity>> SearchAsync(string query);
    Task<IEnumerable<Opportunity>> GetTop10Async();
    Task<(int Count, decimal? EstimatedValue)> GetCountAndEstimatedValueAsync();
}