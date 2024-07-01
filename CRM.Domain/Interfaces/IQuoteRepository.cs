using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;
public interface IQuoteRepository
{
    Task<Quote> GetQuoteByIdAsync(Guid quoteId);
    Task<IEnumerable<Quote>> GetQuoteOpportunituByIdAsync(Guid opportunity);
    Task<IEnumerable<Quote>> GetAllQuotesAsync();
    Task AddQuoteAsync(Quote quoteEntity);
    Task UpdateQuoteAsync(Quote quoteEntity);
    Task DeleteQuoteAsync(Guid quoteId);
    Task<IEnumerable<Quote>> SearchAsync(string query);
    Task<IEnumerable<Quote>> GetTop10Async();

}