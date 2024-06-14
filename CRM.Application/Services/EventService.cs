using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CRM.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventDTO>> GetAllAsync()
    {
        var events = await _eventRepository.GetAllEventsAsync();
        return _mapper.Map<IEnumerable<EventDTO>>(events);
    }

    public async Task<EventDTO> GetByIdAsync(Guid id)
    {
        var evento = await _eventRepository.GetEventByIdAsync(id);
        return _mapper.Map<EventDTO>(evento);
    }

    public async Task AddAsync(EventDTO evento)
    {
        var eventEntity = _mapper.Map<Event>(evento);
        await _eventRepository.AddEventAsync(eventEntity);
    }

    public async Task UpdateAsync(EventDTO evento)
    {
        var eventEntity = _mapper.Map<Event>(evento);
        await _eventRepository.UpdateEventAsync(eventEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _eventRepository.DeleteEventAsync(id);
    }
}