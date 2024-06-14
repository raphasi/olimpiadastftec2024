using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllCustomersAsync();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task AddAsync(CustomerDTO customer)
        {
            var customerEntity = _mapper.Map<Customer>(customer);
            await _customerRepository.AddCustomerAsync(customerEntity);
        }

        public async Task UpdateAsync(CustomerDTO customer)
        {
            var customerEntity = _mapper.Map<Customer>(customer);
            await _customerRepository.UpdateCustomerAsync(customerEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}