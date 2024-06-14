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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            await _customerRepository.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerID }, customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(Guid id, [FromBody] Customer customer)
        {
            if (customer == null || customer.CustomerID != id)
            {
                return BadRequest();
            }

            var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            await _customerRepository.UpdateCustomerAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}