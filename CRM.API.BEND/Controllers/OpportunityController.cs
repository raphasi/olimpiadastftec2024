using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.BEND.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OpportunityController : ControllerBase
{
    private readonly IOpportunityService _opportunityService;

    public OpportunityController(IOpportunityService opportunityService)
    {
        _opportunityService = opportunityService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OpportunityDTO>> GetOpportunityById(Guid id)
    {
        var opportunity = await _opportunityService.GetByIdAsync(id);
        if (opportunity == null)
        {
            return NotFound();
        }
        return Ok(opportunity);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OpportunityDTO>>> GetAllOpportunities()
    {
        var opportunities = await _opportunityService.GetAllAsync();
        return Ok(opportunities);
    }

    [HttpPost]
    public async Task<ActionResult> AddOpportunity([FromBody] OpportunityDTO opportunity)
    {
        if (opportunity == null)
        {
            return BadRequest();
        }

        await _opportunityService.AddAsync(opportunity);
        return CreatedAtAction(nameof(GetOpportunityById), new { id = opportunity.OpportunityID }, opportunity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOpportunity(Guid id, [FromBody] OpportunityDTO opportunity)
    {
        if (opportunity == null || opportunity.OpportunityID != id)
        {
            return BadRequest();
        }

        var existingOpportunity = await _opportunityService.GetByIdAsync(id);
        if (existingOpportunity == null)
        {
            return NotFound();
        }

        await _opportunityService.UpdateAsync(opportunity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOpportunity(Guid id)
    {
        var existingOpportunity = await _opportunityService.GetByIdAsync(id);
        if (existingOpportunity == null)
        {
            return NotFound();
        }

        await _opportunityService.DeleteAsync(id);
        return NoContent();
    }
}