using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CRM.API.BEND.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PriceLevelController : ControllerBase
{
    private readonly IPriceLevelService _priceLevelService;

    public PriceLevelController(IPriceLevelService priceLevelService)
    {
        _priceLevelService = priceLevelService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PriceLevelDTO>> GetPriceLevelById(Guid id)
    {
        var priceLevel = await _priceLevelService.GetByIdAsync(id);
        if (priceLevel == null)
        {
            return NotFound();
        }
        return Ok(priceLevel);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PriceLevelDTO>>> GetAllPriceLevels()
    {
        var priceLevels = await _priceLevelService.GetAllAsync();
        return Ok(priceLevels);
    }

    [HttpPost]
    public async Task<ActionResult> AddPriceLevel([FromBody] PriceLevelDTO priceLevel)
    {
        if (priceLevel == null)
        {
            return BadRequest();
        }

        await _priceLevelService.AddAsync(priceLevel);
        return CreatedAtAction(nameof(GetPriceLevelById), new { id = priceLevel.PriceLevelID }, priceLevel);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePriceLevel(Guid id, [FromBody] PriceLevelDTO priceLevel)
    {
        if (priceLevel == null || priceLevel.PriceLevelID != id)
        {
            return BadRequest();
        }

        var existingPriceLevel = await _priceLevelService.GetByIdAsync(id);
        if (existingPriceLevel == null)
        {
            return NotFound();
        }

        await _priceLevelService.UpdateAsync(priceLevel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePriceLevel(Guid id)
    {
        var existingPriceLevel = await _priceLevelService.GetByIdAsync(id);
        if (existingPriceLevel == null)
        {
            return NotFound();
        }

        await _priceLevelService.DeleteAsync(id);
        return NoContent();
    }
}