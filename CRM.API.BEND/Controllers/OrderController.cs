using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using CRM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly IGenericUpdateService<Order> _genericUpdateService;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger, IGenericUpdateService<Order> genericUpdateService)
        {
            _orderService = orderService;
            _logger = logger;
            _genericUpdateService = genericUpdateService;
        }

        [HttpPatch("{id}/update-field")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateField(Guid id, [FromBody] UpdateFieldDTO updateFieldDTO)
        {
            if (updateFieldDTO == null || string.IsNullOrEmpty(updateFieldDTO.FieldName))
            {
                return BadRequest("Dados do campo são obrigatórios.");
            }

            try
            {
                object fieldValue = null;

                // Verifica o tipo do FieldValue e converte adequadamente
                if (updateFieldDTO.FieldValue.ValueKind == JsonValueKind.Number && updateFieldDTO.FieldValue.TryGetInt32(out int intValue))
                {
                    fieldValue = intValue;
                }
                else if (updateFieldDTO.FieldValue.ValueKind == JsonValueKind.String)
                {
                    fieldValue = updateFieldDTO.FieldValue.GetString();
                }

                await _genericUpdateService.UpdateFieldAsync(id, updateFieldDTO.FieldName, fieldValue);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar campo da entidade.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderDTO>> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Pedido com ID {OrderId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter pedido por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDTO>), 200)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os pedidos.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddOrder([FromBody] OrderDTO order)
        {
            if (order == null)
            {
                return BadRequest("Dados do pedido são obrigatórios.");
            }

            try
            {
                await _orderService.AddAsync(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderID }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar pedido.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] OrderDTO order)
        {
            if (order == null || order.OrderID != id)
            {
                return BadRequest("Dados do pedido são inválidos.");
            }

            try
            {
                await _orderService.UpdateAsync(order);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pedido.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            try
            {
                var existingOrder = await _orderService.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }

                await _orderService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar pedido.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}