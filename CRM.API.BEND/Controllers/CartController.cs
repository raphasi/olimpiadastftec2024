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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CartDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CartDto>> GetCartById(Guid id)
        {
            try
            {
                var cart = await _cartService.GetByIdAsync(id);
                if (cart == null)
                {
                    _logger.LogWarning("Carrinho com ID {CartId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter carrinho por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CartDto>), 200)]
        public async Task<ActionResult<IEnumerable<CartDto>>> GetAllCarts()
        {
            try
            {
                var carts = await _cartService.GetAllAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os carrinhos.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddCart([FromBody] CartDto cart)
        {
            if (cart == null)
            {
                return BadRequest("Dados do carrinho são obrigatórios.");
            }

            try
            {
                await _cartService.AddAsync(cart);
                return CreatedAtAction(nameof(GetCartById), new { id = cart.CartID }, cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar carrinho.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateCart(Guid id, [FromBody] CartDto cart)
        {
            if (cart == null || cart.CartID != id)
            {
                return BadRequest("Dados do carrinho são inválidos.");
            }

            try
            {
                await _cartService.UpdateAsync(cart);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar carrinho.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteCart(Guid id)
        {
            try
            {
                var existingCart = await _cartService.GetByIdAsync(id);
                if (existingCart == null)
                {
                    return NotFound();
                }

                await _cartService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar carrinho.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}