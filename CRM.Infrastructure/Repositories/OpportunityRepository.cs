using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class OpportunityRepository : IOpportunityRepository
    {
        private readonly ApplicationDbContext _opportunityContext;

        public OpportunityRepository(ApplicationDbContext opportunityContext)
        {
            _opportunityContext = opportunityContext;
        }

        public async Task<(int Count, decimal? EstimatedValue)> GetCountAndEstimatedValueAsync()
        {
            var count = await _opportunityContext.Opportunities.CountAsync();
            var estimatedValue = await _opportunityContext.Opportunities.SumAsync(o => o.EstimatedValue);
            return (count, estimatedValue);
        }

        public async Task<Opportunity> GetOpportunityByIdAsync(Guid opportunityId)
        {
            return await _opportunityContext.Set<Opportunity>().FindAsync(opportunityId);
        }

        public async Task<IEnumerable<Opportunity>> GetAllOpportunitiesAsync()
        {
            return await _opportunityContext.Set<Opportunity>()
                .Include(c => c.Lead)
                .Include(c => c.Customer)
                .ToListAsync();
        }

        public async Task AddOpportunityAsync(Opportunity opportunityEntity)
        {
            await _opportunityContext.Set<Opportunity>().AddAsync(opportunityEntity);
            await _opportunityContext.SaveChangesAsync();
        }

        public async Task UpdateOpportunityAsync(Opportunity opportunityEntity)
        {
            _opportunityContext.Set<Opportunity>().Update(opportunityEntity);
            await _opportunityContext.SaveChangesAsync();
        }

        public async Task DeleteOpportunityAsync(Guid opportunityId)
        {
            var opportunity = await _opportunityContext.Set<Opportunity>().FindAsync(opportunityId);
            if (opportunity != null)
            {
                _opportunityContext.Set<Opportunity>().Remove(opportunity);
                await _opportunityContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Opportunity>> SearchAsync(string query)
        {
            return await _opportunityContext.Opportunities
                .Where(c => c.Name.Contains(query))
                .ToListAsync();
        }
        public async Task<IEnumerable<Opportunity>> GetTop10Async()
        {
            return await _opportunityContext.Opportunities
                .OrderByDescending(c => c.CreatedOn)
                .Take(10)
                .ToListAsync();
        }
    }
}