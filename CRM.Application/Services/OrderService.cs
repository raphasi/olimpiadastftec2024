using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> AddAsync(OrderDTO order)
        {
            order.OrderID = Guid.NewGuid();
            var orderEntity = _mapper.Map<Order>(order);
            await _orderRepository.AddOrderAsync(orderEntity);
            return order;
        }

        public async Task UpdateAsync(OrderDTO order)
        {
            var orderEntity = _mapper.Map<Order>(order);
            await _orderRepository.UpdateOrderAsync(orderEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}