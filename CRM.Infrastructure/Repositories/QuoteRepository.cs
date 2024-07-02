using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly ApplicationDbContext _quoteContext;

        public QuoteRepository(ApplicationDbContext quoteContext)
        {
            _quoteContext = quoteContext;
        }

        public async Task<Quote> GetQuoteByIdAsync(Guid quoteId)
        {
            return await _quoteContext.Set<Quote>().FindAsync(quoteId);
        }

        public async Task<IEnumerable<Quote>> GetQuoteOpportunituByIdAsync(Guid opportunityId)
        {
            return await _quoteContext.Set<Quote>()
                                  .Where(q => q.OpportunityID == opportunityId)
                                  .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetAllQuotesAsync()
        {
            return await _quoteContext.Set<Quote>()
                .Include(c => c.Lead)
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .Include(c => c.Event)
                .Include(c => c.Opportunity)
                .ToListAsync();
        }

        public async Task AddQuoteAsync(Quote quoteEntity)
        {
            await _quoteContext.Set<Quote>().AddAsync(quoteEntity);
            await _quoteContext.SaveChangesAsync();
        }

        public async Task UpdateQuoteAsync(Quote quoteEntity)
        {
            _quoteContext.Set<Quote>().Update(quoteEntity);
            await _quoteContext.SaveChangesAsync();
        }

        public async Task DeleteQuoteAsync(Guid quoteId)
        {
            var quote = await _quoteContext.Set<Quote>().FindAsync(quoteId);
            if (quote != null)
            {
                _quoteContext.Set<Quote>().Remove(quote);
                await _quoteContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Quote>> SearchAsync(string query)
        {
            return await _quoteContext.Quotes
                .Where(c => c.Name.Contains(query))
                .ToListAsync();
        }
        public async Task<IEnumerable<Quote>> GetTop10Async()
        {
            return await _quoteContext.Quotes
                .OrderByDescending(c => c.CreatedOn)
                .Take(10)
                .ToListAsync();
        }

    }
}