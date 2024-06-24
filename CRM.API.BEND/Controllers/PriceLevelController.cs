using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceLevelController : ControllerBase
    {
        private readonly IPriceLevelService _priceLevelService;
        private readonly ILogger<PriceLevelController> _logger;

        public PriceLevelController(IPriceLevelService priceLevelService, ILogger<PriceLevelController> logger)
        {
            _priceLevelService = priceLevelService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PriceLevelDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PriceLevelDTO>> GetPriceLevelById(Guid id)
        {
            try
            {
                var priceLevel = await _priceLevelService.GetByIdAsync(id);
                if (priceLevel == null)
                {
                    _logger.LogWarning("Nível de preço com ID {PriceLevelId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(priceLevel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter nível de preço por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PriceLevelDTO>), 200)]
        public async Task<ActionResult<IEnumerable<PriceLevelDTO>>> GetAllPriceLevels()
        {
            try
            {
                var priceLevels = await _priceLevelService.GetAllAsync();
                return Ok(priceLevels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os níveis de preço.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddPriceLevel([FromBody] PriceLevelDTO priceLevel)
        {
            if (priceLevel == null)
            {
                return BadRequest("Dados do nível de preço são obrigatórios.");
            }

            try
            {
                await _priceLevelService.AddAsync(priceLevel);
                return CreatedAtAction(nameof(GetPriceLevelById), new { id = priceLevel.PriceLevelID }, priceLevel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar nível de preço.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdatePriceLevel(Guid id, [FromBody] PriceLevelDTO priceLevel)
        {
            if (priceLevel == null || priceLevel.PriceLevelID != id)
            {
                return BadRequest("Dados do nível de preço são inválidos.");
            }

            try
            {
                await _priceLevelService.UpdateAsync(priceLevel);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar nível de preço.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePriceLevel(Guid id)
        {
            try
            {
                var existingPriceLevel = await _priceLevelService.GetByIdAsync(id);
                if (existingPriceLevel == null)
                {
                    return NotFound();
                }

                await _priceLevelService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar nível de preço.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PriceLevelDTO>>> SearchAsync([FromQuery] string query = null)
        {
            var level = await _priceLevelService.SearchAsync(query);
            return Ok(level);
        }
    }
}