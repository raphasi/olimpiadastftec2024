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
    public class PriceLevelController : ControllerBase
    {
        private readonly IPriceLevelRepository _priceLevelRepository;

        public PriceLevelController(IPriceLevelRepository priceLevelRepository)
        {
            _priceLevelRepository = priceLevelRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PriceLevel>> GetPriceLevelById(Guid id)
        {
            var priceLevel = await _priceLevelRepository.GetPriceLevelByIdAsync(id);
            if (priceLevel == null)
            {
                return NotFound();
            }
            return Ok(priceLevel);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriceLevel>>> GetAllPriceLevels()
        {
            var priceLevels = await _priceLevelRepository.GetAllPriceLevelsAsync();
            return Ok(priceLevels);
        }

        [HttpPost]
        public async Task<ActionResult> AddPriceLevel([FromBody] PriceLevel priceLevel)
        {
            if (priceLevel == null)
            {
                return BadRequest();
            }

            await _priceLevelRepository.AddPriceLevelAsync(priceLevel);
            return CreatedAtAction(nameof(GetPriceLevelById), new { id = priceLevel.PriceLevelID }, priceLevel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePriceLevel(Guid id, [FromBody] PriceLevel priceLevel)
        {
            if (priceLevel == null || priceLevel.PriceLevelID != id)
            {
                return BadRequest();
            }

            var existingPriceLevel = await _priceLevelRepository.GetPriceLevelByIdAsync(id);
            if (existingPriceLevel == null)
            {
                return NotFound();
            }

            await _priceLevelRepository.UpdatePriceLevelAsync(priceLevel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePriceLevel(Guid id)
        {
            var existingPriceLevel = await _priceLevelRepository.GetPriceLevelByIdAsync(id);
            if (existingPriceLevel == null)
            {
                return NotFound();
            }

            await _priceLevelRepository.DeletePriceLevelAsync(id);
            return NoContent();
        }
    }
}