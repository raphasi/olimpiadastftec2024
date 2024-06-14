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
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItemById(Guid id)
        {
            var orderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAllOrderItems()
        {
            var orderItems = await _orderItemRepository.GetAllOrderItemsAsync();
            return Ok(orderItems);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrderItem([FromBody] OrderItem orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest();
            }

            await _orderItemRepository.AddOrderItemAsync(orderItem);
            return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.OrderItemID }, orderItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrderItem(Guid id, [FromBody] OrderItem orderItem)
        {
            if (orderItem == null || orderItem.OrderItemID != id)
            {
                return BadRequest();
            }

            var existingOrderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
            if (existingOrderItem == null)
            {
                return NotFound();
            }

            await _orderItemRepository.UpdateOrderItemAsync(orderItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderItem(Guid id)
        {
            var existingOrderItem = await _orderItemRepository.GetOrderItemByIdAsync(id);
            if (existingOrderItem == null)
            {
                return NotFound();
            }

            await _orderItemRepository.DeleteOrderItemAsync(id);
            return NoContent();
        }
    }
}