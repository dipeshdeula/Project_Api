using Microsoft.AspNetCore.Mvc.Infrastructure;
using Project_Api.Interfaces;
using AutoMapper;
using Project_Api.Dtos;
using Project_Api.Models;
using Project_Api.Utilities;

namespace Project_Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, JwtTokenHelper jwtTokenHelper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _jwtTokenHelper = jwtTokenHelper;

        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomerAsync()
        {
            var customers = await _customerRepository.GetllCustomersAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task AddCustomerAsync(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.Password = _jwtTokenHelper.HashPassword(customer.Password);
            await _customerRepository.AddCustomerAsync(customer);
        }

        public Task UpdateCustomerAsync(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            customer.Password = _jwtTokenHelper.HashPassword(customer.Password);
            return _customerRepository.UpdateCustomerAsync(customer);
        }
        public async Task DeleteCustomerAsync(int id)
        {
            await _customerRepository.DeleteCustomerAsync(id);
        }           

      
    }
}
