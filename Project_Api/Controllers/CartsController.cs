using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Api.Dtos;
using Project_Api.Interfaces;

namespace Project_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartsController> _logger;

        public CartsController(ICartService cartService, ILogger<CartsController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCartByCustomerId(int customerId)
        {
            _logger.LogInformation("Getting cart for customer ID {CustomerId}", customerId);

            var cart = await _cartService.GetCartByCustomerIdAsync(customerId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for customer ID {CustomerId}", customerId);

                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody] CartDto cartDto)
        {
            await _cartService.AddCartAsync(cartDto);
            _logger.LogInformation("Added cart for customer ID {CustomerId}", cartDto.CustomerId);

            return CreatedAtAction(nameof(GetCartByCustomerId), new { customerId = cartDto.CustomerId }, cartDto);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] CartDto cartDto)
        {
            if (id != cartDto.Id)
            {
                _logger.LogWarning("Cart ID mismatch: {Id} != {CartDtoId}", id, cartDto.Id);

                return BadRequest();
            }

            await _cartService.UpdateCartAsync(cartDto);
            _logger.LogInformation("Updated cart ID {CartId}", id);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            await _cartService.DeleteCartAsync(id);
            _logger.LogInformation("Deleted cart ID {CartId}", id);

            return NoContent();
        }

        [HttpPost("{customerId}/items")]
        public async Task<ActionResult> AddCartItem(int customerId, [FromBody]CartItemDto cartItemDto)
        {
            await _cartService.AddCartItemAsync(customerId, cartItemDto);
            _logger.LogInformation("Added item to cart for customer ID {CustomerId}", customerId);

            return NoContent();
        }

        [HttpPut("{customerId}/items/{itemId}")]
        public async Task<IActionResult> UpdateCartItem(int customerId, int itemId, [FromBody] CartItemDto cartItemDto)
        {
            if (itemId != cartItemDto.Id)
            {
                _logger.LogWarning("Cart item ID mismatch: {ItemId} != {CartItemDtoId}", itemId, cartItemDto.Id);
                return BadRequest();
            }

            await _cartService.UpdateCartItemAsync(customerId, cartItemDto);
            _logger.LogInformation("Updated cart item ID {ItemId} for customer ID {CustomerId}", itemId, customerId);

            return NoContent();
        }


        [HttpGet("{customerId}/items")]
        public async Task<IActionResult> GetCartItemsByCustomerId(int customerId)
        {
            _logger.LogInformation("Getting cart items for customer ID {CustomerId}", customerId);

            var cartItems = await _cartService.GetCartItemsByCustomerIdAsync(customerId);
            return Ok(cartItems);
        }

    }
}
