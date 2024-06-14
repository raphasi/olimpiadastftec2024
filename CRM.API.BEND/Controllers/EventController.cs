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
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEventById(Guid id)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(id);
            if (eventEntity == null)
            {
                return NotFound();
            }
            return Ok(eventEntity);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult> AddEvent([FromBody] Event eventEntity)
        {
            if (eventEntity == null)
            {
                return BadRequest();
            }

            await _eventRepository.AddEventAsync(eventEntity);
            return CreatedAtAction(nameof(GetEventById), new { id = eventEntity.EventID }, eventEntity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] Event eventEntity)
        {
            if (eventEntity == null || eventEntity.EventID != id)
            {
                return BadRequest();
            }

            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            await _eventRepository.UpdateEventAsync(eventEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(Guid id)
        {
            var existingEvent = await _eventRepository.GetEventByIdAsync(id);
            if (existingEvent == null)
            {
                return NotFound();
            }

            await _eventRepository.DeleteEventAsync(id);
            return NoContent();
        }
    }
}