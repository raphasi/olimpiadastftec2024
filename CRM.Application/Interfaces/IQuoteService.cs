using CRM.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IQuoteService
{
    Task<IEnumerable<QuoteDTO>> GetAllAsync();
    Task<QuoteDTO> GetByIdAsync(Guid id);
    Task AddAsync(QuoteDTO quote);
    Task UpdateAsync(QuoteDTO quote);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<QuoteDTO>> SearchAsync(string query);
}
