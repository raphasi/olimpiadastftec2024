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
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IProductEventService _productEventService;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventService, IProductEventService productEventService, ILogger<EventController> logger)
        {
            _eventService = eventService;
            _productEventService = productEventService;
            _logger = logger;
        }



        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EventDTO>> GetEventById(Guid id)
        {
            try
            {
                var eventItem = await _eventService.GetByIdAsync(id);
                if (eventItem == null)
                {
                    _logger.LogWarning("Evento com ID {EventId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(eventItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter evento por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os eventos.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddEvent([FromBody] EventDTO eventItem)
        {
            if (eventItem == null)
            {
                return BadRequest("Dados do evento são obrigatórios.");
            }

            try
            {
                await _eventService.AddAsync(eventItem);
                return CreatedAtAction(nameof(GetEventById), new { id = eventItem.EventID }, eventItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar evento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] EventDTO eventItem)
        {
            if (eventItem == null || eventItem.EventID != id)
            {
                return BadRequest("Dados do evento são inválidos.");
            }

            try
            {
                await _eventService.UpdateAsync(eventItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar evento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            try
            {
                var existingEvent = await _eventService.GetByIdAsync(id);
                if (existingEvent == null)
                {
                    return NotFound();
                }

                await _eventService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar evento.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> SearchCustomers([FromQuery] string query = null)
        {
            var customers = await _eventService.SearchAsync(query);
            return Ok(customers);
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<ProductEventDTO>>> GetProductsByEventId(Guid id)
        {
            var products = await _productEventService.GetProductsByEventIdAsync(id);
            return Ok(products);
        }
    }
}