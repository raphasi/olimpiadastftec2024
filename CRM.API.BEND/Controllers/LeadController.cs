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
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;
        private readonly ILogger<LeadController> _logger;
        private readonly IGenericUpdateService<Lead> _genericUpdateService;

        public LeadController(ILeadService leadService, ILogger<LeadController> logger, IGenericUpdateService<Lead> genericUpdateService)
        {
            _leadService = leadService;
            _logger = logger;
            _genericUpdateService = genericUpdateService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LeadDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LeadDTO>> GetLeadById(Guid id)
        {
            try
            {
                var lead = await _leadService.GetByIdAsync(id);
                if (lead == null)
                {
                    _logger.LogWarning("Lead com ID {LeadId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(lead);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lead por ID.");
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
        [ProducesResponseType(typeof(IEnumerable<LeadDTO>), 200)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LeadDTO>>> GetAllLeads()
        {
            try
            {
                var leads = await _leadService.GetAllAsync();
                return Ok(leads);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os leads.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddLead([FromBody] LeadDTO lead)
        {
            if (lead == null)
            {
                return BadRequest("Dados do lead são obrigatórios.");
            }

            try
            {
                var createdLead = await _leadService.AddAsync(lead);
                return CreatedAtAction(nameof(GetLeadById), new { id = createdLead.LeadID }, createdLead);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar lead.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateLead(Guid id, [FromBody] LeadDTO lead)
        {
            if (lead == null || lead.LeadID != id)
            {
                return BadRequest("Dados do lead são inválidos.");
            }

            try
            {
                await _leadService.UpdateAsync(lead);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar lead.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteLead(Guid id)
        {
            try
            {
                var existingLead = await _leadService.GetByIdAsync(id);
                if (existingLead == null)
                {
                    return NotFound();
                }

                await _leadService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar lead.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<LeadDTO>>> SearchAsync([FromQuery] string query = null)
        {
            var leads = await _leadService.SearchAsync(query);
            return Ok(leads);
        }
    }
}