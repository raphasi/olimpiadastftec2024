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

    public async Task<QuoteDTO> GetByIdAsync(Guid id)
    {
        var quote = await _quoteRepository.GetQuoteByIdAsync(id);
        return _mapper.Map<QuoteDTO>(quote);
    }

    public async Task AddAsync(QuoteDTO quote)
    {
        var quoteEntity = _mapper.Map<Quote>(quote);
        await _quoteRepository.AddQuoteAsync(quoteEntity);
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
}