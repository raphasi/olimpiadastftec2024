using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuoteController(IQuoteService quoteRepository)
        {
            _quoteService = quoteRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuoteDTO>> GetQuoteById(Guid id)
        {
            var quote = await _quoteService.GetByIdAsync(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuoteDTO>>> GetAllQuotes()
        {
            var quotes = await _quoteService.GetAllAsync();
            return Ok(quotes);
        }

        [HttpPost]
        public async Task<ActionResult> AddQuote([FromBody] QuoteDTO quote)
        {
            if (quote == null)
            {
                return BadRequest();
            }

            await _quoteService.AddAsync(quote);
            return CreatedAtAction(nameof(GetQuoteById), new { id = quote.QuoteID }, quote);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateQuote(Guid id, [FromBody] QuoteDTO quote)
        {
            if (quote == null || quote.QuoteID != id)
            {
                return BadRequest();
            }

            var existingQuote = await _quoteService.GetByIdAsync(id);
            if (existingQuote == null)
            {
                return NotFound();
            }

            await _quoteService.UpdateAsync(quote);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuote(Guid id)
        {
            var existingQuote = await _quoteService.GetByIdAsync(id);
            if (existingQuote == null)
            {
                return NotFound();
            }

            await _quoteService.DeleteAsync(id);
            return NoContent();
        }
    }
}