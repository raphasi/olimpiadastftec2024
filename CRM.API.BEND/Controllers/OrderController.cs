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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            await _orderRepository.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderID }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] Order order)
        {
            if (order == null || order.OrderID != id)
            {
                return BadRequest();
            }

            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderRepository.UpdateOrderAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderRepository.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}