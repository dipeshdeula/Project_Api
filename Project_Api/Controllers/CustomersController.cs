using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Api.Dtos;
using Project_Api.Interfaces;

namespace Project_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomer()
        {
            var customers = await _customerService.GetAllCustomerAsync();
            return Ok(customers);

        }
        [Authorize(Roles ="Admin,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromForm]CustomerDto customerDto)
        {

            await _customerService.AddCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerDto.Id }, customerDto);
           

        }
        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer([FromForm]int id, [FromForm]CustomerDto customerDto)
        { 
            if(id != customerDto.Id)
            {
                return BadRequest();
            }


            await _customerService.UpdateCustomerAsync(customerDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}
