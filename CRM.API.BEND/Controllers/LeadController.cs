using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;
        private readonly ILogger<LeadController> _logger;

        public LeadController(ILeadService leadService, ILogger<LeadController> logger)
        {
            _leadService = leadService;
            _logger = logger;
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
            catch (Exception ex)            {
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
                await _leadService.AddAsync(lead);
                return CreatedAtAction(nameof(GetLeadById), new { id = lead.LeadID }, lead);
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