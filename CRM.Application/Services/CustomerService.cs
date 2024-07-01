using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllCustomersAsync();
                return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os clientes.");
                throw;
            }
        }

        public async Task<CustomerDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    _logger.LogWarning("Cliente com ID {CustomerId} não encontrado.", id);
                    return null;
                }
                return _mapper.Map<CustomerDTO>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter cliente por ID.");
                throw;
            }
        }

        public async Task<CustomerDTO> AddAsync(CustomerDTO customer)
        {
            try
            {
                customer.CustomerID = Guid.NewGuid();
                var customerEntity = _mapper.Map<Customer>(customer);
                await _customerRepository.AddCustomerAsync(customerEntity);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar cliente.");
                throw;
            }
        }

        public async Task UpdateAsync(CustomerDTO customer)
        {
            try
            {
                var customerEntity = _mapper.Map<Customer>(customer);
                _customerRepository.DetachCustomerAsync(customerEntity);
                await _customerRepository.UpdateCustomerAsync(customerEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cliente.");
                throw;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _customerRepository.DeleteCustomerAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar cliente.");
                throw;
            }
        }
        public async Task<IEnumerable<CustomerDTO>> SearchAsync(string query)
        {
            var customers = string.IsNullOrEmpty(query)
                ? await _customerRepository.GetTop10Async()
                : await _customerRepository.SearchAsync(query);
            return customers.Select(c => new CustomerDTO
            {
                CustomerID = c.CustomerID,
                FullName = c.FullName,
                Email = c.Email,
                CPF = c.CPF,
                CNPJ = c.CNPJ
            });
        }
    }
}