using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuoteById(Guid id)
        {
            var quote = await _quoteRepository.GetQuoteByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetAllQuotes()
        {
            var quotes = await _quoteRepository.GetAllQuotesAsync();
            return Ok(quotes);
        }

        [HttpPost]
        public async Task<ActionResult> AddQuote([FromBody] Quote quote)
        {
            if (quote == null)
            {
                return BadRequest();
            }

            await _quoteRepository.AddQuoteAsync(quote);
            return CreatedAtAction(nameof(GetQuoteById), new { id = quote.QuoteID }, quote);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuote(Guid id, [FromBody] Quote quote)
        {
            if (quote == null || quote.QuoteID != id)
            {
                return BadRequest();
            }

            var existingQuote = await _quoteRepository.GetQuoteByIdAsync(id);
            if (existingQuote == null)
            {
                return NotFound();
            }

            await _quoteRepository.UpdateQuoteAsync(quote);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuote(Guid id)
        {
            var existingQuote = await _quoteRepository.GetQuoteByIdAsync(id);
            if (existingQuote == null)
            {
                return NotFound();
            }

            await _quoteRepository.DeleteQuoteAsync(id);
            return NoContent();
        }
    }
}