using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        private readonly ILogger<OrderItemController> _logger;

        public OrderItemController(IOrderItemService orderItemService, ILogger<OrderItemController> logger)
        {
            _orderItemService = orderItemService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderItemDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderItemDTO>> GetOrderItemById(Guid id)
        {
            try
            {
                var orderItem = await _orderItemService.GetByIdAsync(id);
                if (orderItem == null)
                {
                    _logger.LogWarning("Item do pedido com ID {OrderItemId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter item do pedido por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderItemDTO>), 200)]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetAllOrderItems()
        {
            try
            {
                var orderItems = await _orderItemService.GetAllAsync();
                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os itens do pedido.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddOrderItem([FromBody] OrderItemDTO orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest("Dados do item do pedido são obrigatórios.");
            }

            try
            {
                await _orderItemService.AddAsync(orderItem);
                return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.OrderItemID }, orderItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar item do pedido.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOrderItem(Guid id, [FromBody] OrderItemDTO orderItem)
        {
            if (orderItem == null || orderItem.OrderItemID != id)
            {
                return BadRequest("Dados do item do pedido são inválidos.");
            }

            try
            {
                await _orderItemService.UpdateAsync(orderItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar item do pedido.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOrderItem(Guid id)
        {
            try
            {
                var existingOrderItem = await _orderItemService.GetByIdAsync(id);
                if (existingOrderItem == null)
                {
                    return NotFound();
                }

                await _orderItemService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar item do pedido.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}