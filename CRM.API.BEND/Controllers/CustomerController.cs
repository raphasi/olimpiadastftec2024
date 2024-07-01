using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning("Cliente com ID {CustomerId} não encontrado.", id);
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter cliente por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerDTO>), 200)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os clientes.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddCustomer([FromBody] CustomerDTO customer)
        {
            if (customer == null)
            {
                return BadRequest("Dados do cliente são obrigatórios.");
            }

            try
            {
                await _customerService.AddAsync(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerID }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar cliente.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDTO customer)
        {
            if (customer == null || customer.CustomerID != id)
            {
                return BadRequest("Dados do cliente são inválidos.");
            }

            try
            {
                //var existingCustomer = await _customerService.GetByIdAsync(id);
                //if (existingCustomer == null)
                //{
                //    return NotFound();
                //}
                // Desanexar a entidade anterior
                await _customerService.UpdateAsync(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cliente.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var existingCustomer = await _customerService.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar cliente.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> SearchCustomers([FromQuery] string query = null)
        {
            var customers = await _customerService.SearchAsync(query);
            return Ok(customers);
        }
    }
}