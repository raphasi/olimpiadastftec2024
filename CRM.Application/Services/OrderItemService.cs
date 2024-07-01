using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CRM.Application.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;

    public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderItemDTO>> GetAllAsync()
    {
        var orderItems = await _orderItemRepository.GetAllOrderItemsAsync();
        return _mapper.Map<IEnumerable<OrderItemDTO>>(orderItems);
    }

    public async Task<OrderItemDTO> GetByIdAsync(Guid id)
    {
        var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
        return _mapper.Map<OrderItemDTO>(orderItem);
    }

    public async Task<OrderItemDTO> AddAsync(OrderItemDTO orderItem)
    {
        orderItem.OrderItemID = Guid.NewGuid();
        var orderItemEntity = _mapper.Map<OrderItem>(orderItem);
        await _orderItemRepository.AddOrderItemAsync(orderItemEntity);
        return orderItem;
    }

    public async Task UpdateAsync(OrderItemDTO orderItem)
    {
        var orderItemEntity = _mapper.Map<OrderItem>(orderItem);
        await _orderItemRepository.UpdateOrderItemAsync(orderItemEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _orderItemRepository.DeleteOrderItemAsync(id);
    }
}