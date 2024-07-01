using AutoMapper;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class ProductEventService : IProductEventService
    {
        private readonly IProductEventRepository _productEventRepository;
        private readonly IMapper _mapper;

        public ProductEventService(IProductEventRepository productEventRepository, IMapper mapper)
        {
            _productEventRepository = productEventRepository;
            _mapper = mapper;
        }

        public async Task<ProductEventDTO> GetByIdAsync(Guid productEventId)
        {
            var productEvent = await _productEventRepository.GetByIdAsync(productEventId);
            return _mapper.Map<ProductEventDTO>(productEvent);
        }

        public async Task<IEnumerable<ProductEventDTO>> GetAllAsync()
        {
            var productEvents = await _productEventRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductEventDTO>>(productEvents);
        }

        public async Task<ProductEventDTO> AddAsync(ProductEventDTO createProductEventDto)
        {
            createProductEventDto.Id = Guid.NewGuid();
            var productEvent = _mapper.Map<ProductEvent>(createProductEventDto);
            await _productEventRepository.AddAsync(productEvent);
            return createProductEventDto;
        }

        public async Task UpdateAsync(ProductEventDTO updateProductEventDto)
        {
            var productEvent = _mapper.Map<ProductEvent>(updateProductEventDto);
            await _productEventRepository.UpdateAsync(productEvent);
        }

        public async Task DeleteAsync(Guid productEventId)
        {
            await _productEventRepository.DeleteAsync(productEventId);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByEventIdAsync(Guid eventId)
        {
            var productEvents = await _productEventRepository.GetByEventIdAsync(eventId);
            var products = new List<ProductDTO>();

            foreach (var productEvent in productEvents)
            {
                products.Add(_mapper.Map<ProductDTO>(productEvent.Product));
            }

            return products;
        }

        public async Task<IEnumerable<EventDTO>> GetEventsByProductIdAsync(Guid productId)
        {
            var productEvents = await _productEventRepository.GetByProductIdAsync(productId);
            var events = new List<EventDTO>();

            foreach (var productEvent in productEvents)
            {
                events.Add(_mapper.Map<EventDTO>(productEvent.Event));
            }

            return events;
        }
    }
}