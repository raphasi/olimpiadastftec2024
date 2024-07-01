using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDTO>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Produto com ID {ProductId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter produto por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os produtos.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddProduct([FromBody] ProductDTO product)
        {
            if (product == null)
            {
                return BadRequest("Dados do produto são obrigatórios.");
            }

            try
            {
                await _productService.AddAsync(product);
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar produto.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO product)
        {
            if (product == null || product.ProductID != id)
            {
                return BadRequest("Dados do produto são inválidos.");
            }

            try
            {
                await _productService.UpdateAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar produto.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var existingProduct = await _productService.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar produto.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchAsync([FromQuery] string query = null)
        {
            var prd = await _productService.SearchAsync(query);
            return Ok(prd);
        }


    }
}