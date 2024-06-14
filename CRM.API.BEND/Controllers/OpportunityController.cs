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
    public class OpportunityController : ControllerBase
    {
        private readonly IOpportunityRepository _opportunityRepository;

        public OpportunityController(IOpportunityRepository opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Opportunity>> GetOpportunityById(Guid id)
        {
            var opportunity = await _opportunityRepository.GetOpportunityByIdAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }
            return Ok(opportunity);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Opportunity>>> GetAllOpportunities()
        {
            var opportunities = await _opportunityRepository.GetAllOpportunitiesAsync();
            return Ok(opportunities);
        }

        [HttpPost]
        public async Task<ActionResult> AddOpportunity([FromBody] Opportunity opportunity)
        {
            if (opportunity == null)
            {
                return BadRequest();
            }

            await _opportunityRepository.AddOpportunityAsync(opportunity);
            return CreatedAtAction(nameof(GetOpportunityById), new { id = opportunity.OpportunityID }, opportunity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOpportunity(Guid id, [FromBody] Opportunity opportunity)
        {
            if (opportunity == null || opportunity.OpportunityID != id)
            {
                return BadRequest();
            }

            var existingOpportunity = await _opportunityRepository.GetOpportunityByIdAsync(id);
            if (existingOpportunity == null)
            {
                return NotFound();
            }

            await _opportunityRepository.UpdateOpportunityAsync(opportunity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOpportunity(Guid id)
        {
            var existingOpportunity = await _opportunityRepository.GetOpportunityByIdAsync(id);
            if (existingOpportunity == null)
            {
                return NotFound();
            }

            await _opportunityRepository.DeleteOpportunityAsync(id);
            return NoContent();
        }
    }
}