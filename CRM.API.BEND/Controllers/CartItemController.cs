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
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartItemController> _logger;

        public CartItemController(ICartItemService cartItemService, ILogger<CartItemController> logger)
        {
            _cartItemService = cartItemService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CartItemDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CartItemDTO>> GetCartItemById(Guid id)
        {
            try
            {
                var cartItem = await _cartItemService.GetByIdAsync(id);
                if (cartItem == null)
                {
                    _logger.LogWarning("Item do carrinho com ID {CartItemId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter item do carrinho por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("cart/{cartId}")]
        [ProducesResponseType(typeof(IEnumerable<CartItemDTO>), 200)]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetAllCartItemsByCartId(Guid cartId)
        {
            try
            {
                var cartItems = await _cartItemService.GetAllByCartIdAsync(cartId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os itens do carrinho.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddCartItem([FromBody] CartItemDTO cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest("Dados do item do carrinho são obrigatórios.");
            }

            try
            {
                await _cartItemService.AddAsync(cartItem);
                return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.CartItemID }, cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar item ao carrinho.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateCartItem(Guid id, [FromBody] CartItemDTO cartItem)
        {
            if (cartItem == null || cartItem.CartItemID != id)
            {
                return BadRequest("Dados do item do carrinho são inválidos.");
            }

            try
            {
                await _cartItemService.UpdateAsync(cartItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar item do carrinho.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteCartItem(Guid id)
        {
            try
            {
                var existingCartItem = await _cartItemService.GetByIdAsync(id);
                if (existingCartItem == null)
                {
                    return NotFound();
                }

                await _cartItemService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar item do carrinho.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}