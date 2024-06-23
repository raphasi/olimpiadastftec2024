using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly ILogger<ActivityController> _logger;

        public ActivityController(IActivityService activityService, ILogger<ActivityController> logger)
        {
            _activityService = activityService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActivityDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ActivityDTO>> GetActivityById(Guid id)
        {
            try
            {
                var activity = await _activityService.GetByIdAsync(id);
                if (activity == null)
                {
                    _logger.LogWarning("Atividade com ID {ActivityId} não encontrada.", id);
                    return NotFound();
                }
                return Ok(activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter atividade por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ActivityDTO>), 200)]
        public async Task<ActionResult<IEnumerable<ActivityDTO>>> GetAllActivities()
        {
            try
            {
                var activities = await _activityService.GetAllAsync();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as atividades.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddActivity([FromBody] ActivityDTO activity)
        {
            if (activity == null)
            {
                return BadRequest("Dados da atividade são obrigatórios.");
            }

            try
            {
                await _activityService.AddAsync(activity);
                return CreatedAtAction(nameof(GetActivityById), new { id = activity.ActivityID }, activity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar atividade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateActivity(Guid id, [FromBody] ActivityDTO activity)
        {
            if (activity == null || activity.ActivityID != id)
            {
                return BadRequest("Dados da atividade são inválidos.");
            }

            try
            {
                await _activityService.UpdateAsync(activity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar atividade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteActivity(Guid id)
        {
            try
            {
                var existingActivity = await _activityService.GetByIdAsync(id);
                if (existingActivity == null)
                {
                    return NotFound();
                }

                await _activityService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar atividade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}