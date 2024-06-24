using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly ApplicationDbContext _leadContext;

        public LeadRepository(ApplicationDbContext leadContext)
        {
            _leadContext = leadContext;
        }

        public async Task<Lead> GetLeadByIdAsync(Guid leadId)
        {
            return await _leadContext.Set<Lead>().FindAsync(leadId);
        }

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync()
        {
            return await _leadContext.Set<Lead>().ToListAsync();
        }

        public async Task AddLeadAsync(Lead leadEntity)
        {
            await _leadContext.Set<Lead>().AddAsync(leadEntity);
            await _leadContext.SaveChangesAsync();
        }

        public async Task UpdateLeadAsync(Lead leadEntity)
        {
            _leadContext.Set<Lead>().Update(leadEntity);
            await _leadContext.SaveChangesAsync();
        }

        public async Task DeleteLeadAsync(Guid leadId)
        {
            var lead = await _leadContext.Set<Lead>().FindAsync(leadId);
            if (lead != null)
            {
                _leadContext.Set<Lead>().Remove(lead);
                await _leadContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Lead>> SearchAsync(string query)
        {
            return await _leadContext.Leads
                .Where(c => c.FullName.Contains(query) || c.Email.Contains(query))
                .ToListAsync();
        }
        public async Task<IEnumerable<Lead>> GetTop10Async()
        {
            return await _leadContext.Leads
                .OrderByDescending(c => c.CreatedOn)
                .Take(10)
                .ToListAsync();
        }
    }
}