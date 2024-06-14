using Microsoft.AspNetCore.Mvc;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;

        public LeadController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeadDTO>>> GetAllLeads()
        {
            var leads = await _leadService.GetAllAsync();
            return Ok(leads);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeadDTO>> GetLeadById(Guid id)
        {
            var lead = await _leadService.GetByIdAsync(id);
            if (lead == null)
            {
                return NotFound();
            }
            return Ok(lead);
        }

        [HttpPost]
        public async Task<ActionResult> AddLead([FromBody] LeadDTO leadDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _leadService.AddAsync(leadDto);
            return CreatedAtAction(nameof(GetLeadById), new { id = leadDto.LeadID }, leadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLead(Guid id, [FromBody] LeadDTO leadDto)
        {
            if (id != leadDto.LeadID)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _leadService.UpdateAsync(leadDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLead(Guid id)
        {
            await _leadService.DeleteAsync(id);
            return NoContent();
        }
    }
}