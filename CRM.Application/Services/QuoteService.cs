using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services;

public class QuoteService : IQuoteService
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly IMapper _mapper;

    public QuoteService(IQuoteRepository quoteRepository, IMapper mapper)
    {
        _quoteRepository = quoteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuoteDTO>> GetAllAsync()
    {
        var quotes = await _quoteRepository.GetAllQuotesAsync();
        return _mapper.Map<IEnumerable<QuoteDTO>>(quotes);
    }

    public async Task<IEnumerable<QuoteDTO>> GetQuoteOpportunituByIdAsync(Guid oppId)
    {
        var quotes = await _quoteRepository.GetQuoteOpportunituByIdAsync(oppId);
        return _mapper.Map<IEnumerable<QuoteDTO>>(quotes);
    }

    public async Task<QuoteDTO> GetByIdAsync(Guid id)
    {
        var quote = await _quoteRepository.GetQuoteByIdAsync(id);
        return _mapper.Map<QuoteDTO>(quote);
    }

    public async Task<QuoteDTO> AddAsync(QuoteDTO quote)
    {
        quote.QuoteID = Guid.NewGuid();
        var quoteEntity = _mapper.Map<Quote>(quote);
        await _quoteRepository.AddQuoteAsync(quoteEntity);
        return quote;
    }

    public async Task UpdateAsync(QuoteDTO quote)
    {
        var quoteEntity = _mapper.Map<Quote>(quote);
        await _quoteRepository.UpdateQuoteAsync(quoteEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _quoteRepository.DeleteQuoteAsync(id);
    }

    public async Task<IEnumerable<QuoteDTO>> SearchAsync(string query)
    {
        var opps = string.IsNullOrEmpty(query)
            ? await _quoteRepository.GetTop10Async()
            : await _quoteRepository.SearchAsync(query);
        return opps.Select(c => new QuoteDTO
        {
            QuoteID = c.QuoteID,
            Name = c.Name
        });
    }
}