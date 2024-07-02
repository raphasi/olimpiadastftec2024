using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using CRM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly IOpportunityService _opportunityService;
        private readonly ILogger<OpportunityController> _logger;
        private readonly IGenericUpdateService<Opportunity> _genericUpdateService;

        public OpportunityController(IOpportunityService opportunityService, ILogger<OpportunityController> logger, IGenericUpdateService<Opportunity> genericUpdateService)
        {
            _opportunityService = opportunityService;
            _logger = logger;
            _genericUpdateService = genericUpdateService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OpportunityDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OpportunityDTO>> GetOpportunityById(Guid id)
        {
            try
            {
                var opportunity = await _opportunityService.GetByIdAsync(id);
                if (opportunity == null)
                {
                    _logger.LogWarning("Oportunidade com ID {OpportunityId} não encontrada.", id);
                    return NotFound();
                }
                return Ok(opportunity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter oportunidade por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPatch("{id}/update-field")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateField(Guid id, [FromBody] UpdateFieldDTO updateFieldDTO)
        {
            if (updateFieldDTO == null || string.IsNullOrEmpty(updateFieldDTO.FieldName))
            {
                return BadRequest("Dados do campo são obrigatórios.");
            }

            try
            {
                object fieldValue = null;

                // Verifica o tipo do FieldValue e converte adequadamente
                if (updateFieldDTO.FieldValue.ValueKind == JsonValueKind.Number && updateFieldDTO.FieldValue.TryGetInt32(out int intValue))
                {
                    fieldValue = intValue;
                }
                else if (updateFieldDTO.FieldValue.ValueKind == JsonValueKind.String)
                {
                    fieldValue = updateFieldDTO.FieldValue.GetString();
                }

                await _genericUpdateService.UpdateFieldAsync(id, updateFieldDTO.FieldName, fieldValue);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar campo da entidade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OpportunityDTO>), 200)]
        public async Task<ActionResult<IEnumerable<OpportunityDTO>>> GetAllOpportunities()
        {
            try
            {
                var opportunities = await _opportunityService.GetAllAsync();
                return Ok(opportunities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as oportunidades.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddOpportunity([FromBody] OpportunityDTO opportunity)
        {
            if (opportunity == null)
            {
                return BadRequest("Dados da oportunidade são obrigatórios.");
            }

            try
            {
                await _opportunityService.AddAsync(opportunity);
                return CreatedAtAction(nameof(GetOpportunityById), new { id = opportunity.OpportunityID }, opportunity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar oportunidade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOpportunity(Guid id, [FromBody] OpportunityDTO opportunity)
        {
            if (opportunity == null || opportunity.OpportunityID != id)
            {
                return BadRequest("Dados da oportunidade são inválidos.");
            }

            try
            {
                await _opportunityService.UpdateAsync(opportunity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar oportunidade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOpportunity(Guid id)
        {
            try
            {
                var existingOpportunity = await _opportunityService.GetByIdAsync(id);
                if (existingOpportunity == null)
                {
                    return NotFound();
                }

                await _opportunityService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar oportunidade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("count-and-estimated-value")]
        [ProducesResponseType(typeof((int Count, decimal EstimatedValue)), 200)]
        public async Task<ActionResult<(int Count, decimal EstimatedValue)>> GetCountAndEstimatedValue()
        {
            try
            {
                var result = await _opportunityService.GetCountAndEstimatedValueAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contagem e valor estimado das oportunidades.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OpportunityDTO>>> SearchAsync([FromQuery] string query = null)
        {
            var opps = await _opportunityService.SearchAsync(query);
            return Ok(opps);
        }

    }
}