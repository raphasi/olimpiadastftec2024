using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace CRM.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(IEventRepository eventRepository, IProductRepository productRepository, IMapper mapper, ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EventDTO>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            var eventDtos = _mapper.Map<IEnumerable<EventDTO>>(events);
            foreach (var eventDto in eventDtos)
            {
                var eventEntity = events.First(e => e.EventID == eventDto.EventID);
                if (eventEntity.ProductEvents != null)
                    eventDto.SelectedProductIds = eventEntity.ProductEvents.Select(pe => pe.ProductID).ToList();
            }
            return eventDtos;
        }

        public async Task<EventDTO> GetByIdAsync(Guid id)
        {
            var evento = await _eventRepository.GetEventByIdAsync(id);
            var eventDto = _mapper.Map<EventDTO>(evento);
            if (evento.ProductEvents != null)
                eventDto.SelectedProductIds = evento.ProductEvents.Select(pe => pe.ProductID).ToList();
            return eventDto;
        }

        public async Task<EventDTO> AddAsync(EventDTO evento)
        {
            evento.EventID = Guid.NewGuid();
            var eventEntity = _mapper.Map<Event>(evento);
            eventEntity.ProductEvents = evento.SelectedProductIds.Select(productId => new ProductEvent
            {
                ProductID = productId,
                EventID = eventEntity.EventID
            }).ToList();

            await _eventRepository.AddEventAsync(eventEntity);
            return evento;
        }

        public async Task UpdateAsync(EventDTO evento)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(evento.EventID);
            if (eventEntity == null)
            {
                _logger.LogError("", "Erro ao obter todos os eventos.");
            }

            _mapper.Map(evento, eventEntity);
            eventEntity.ProductEvents = evento.SelectedProductIds.Select(productId => new ProductEvent
            {
                ProductID = productId,
                EventID = eventEntity.EventID
            }).ToList();

            await _eventRepository.UpdateEventAsync(eventEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _eventRepository.DeleteEventAsync(id);
        }

        public async Task<IEnumerable<EventDTO>> SearchAsync(string query)
        {
            var opps = string.IsNullOrEmpty(query)
                ? await _eventRepository.GetTop10Async()
                : await _eventRepository.SearchAsync(query);
            return opps.Select(c => new EventDTO
            {
                EventID = c.EventID,
                Name = c.Name
            });
        }
    }
}