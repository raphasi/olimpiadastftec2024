using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.BEND.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> GetEventById(Guid id)
    {
        var eventEntity = await _eventService.GetByIdAsync(id);
        if (eventEntity == null)
        {
            return NotFound();
        }
        return Ok(eventEntity);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
    {
        var events = await _eventService.GetAllAsync();
        return Ok(events);
    }

    [HttpPost]
    public async Task<ActionResult> AddEvent([FromBody] EventDTO eventEntity)
    {
        if (eventEntity == null)
        {
            return BadRequest();
        }

        await _eventService.AddAsync(eventEntity);
        return CreatedAtAction(nameof(GetEventById), new { id = eventEntity.EventID }, eventEntity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEvent(Guid id, [FromBody] EventDTO eventEntity)
    {
        if (eventEntity == null || eventEntity.EventID != id)
        {
            return BadRequest();
        }

        var existingEvent = await _eventService.GetByIdAsync(id);
        if (existingEvent == null)
        {
            return NotFound();
        }

        await _eventService.UpdateAsync(eventEntity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(Guid id)
    {
        var existingEvent = await _eventService.GetByIdAsync(id);
        if (existingEvent == null)
        {
            return NotFound();
        }

        await _eventService.DeleteAsync(id);
        return NoContent();
    }
}