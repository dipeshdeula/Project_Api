using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project_Api.Data;
using Project_Api.Dtos;
using Project_Api.Interfaces;
using Project_Api.Models;

namespace Project_Api.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(ApplicationDbContext context,ICartRepository cartRepository, IMapper mapper,ILogger<CartService> logger)
        {
            _context = context;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<CartDto> GetCartByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation("Getting cart for customer ID {CutomerId}", customerId);
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task AddCartAsync(CartDto cartDto)
        {
            // check if the customer exists
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == cartDto.CustomerId);

            if (!customerExists)
            {
                throw new ArgumentException("Customer does not exist");
            }
            var cart = _mapper.Map<Cart>(cartDto);
           
            await _cartRepository.AddCartAsync(cart);
            _logger.LogInformation("Added cart for customer Id {CustomerId}", cartDto.CustomerId);
            
           

        }

        public async Task UpdateCartAsync(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            try
            {
                await _cartRepository.UpdateCartAsync(cart);
                _logger.LogInformation("Updated cart ID {CartID}", cartDto.Id);
            }

            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex,"Concurrency exception when updating cart ID {CartId}",cartDto.Id);

                //Handle concurrency exception
                throw new Exception("The cart was updated by another user. Please reload and try again.", ex);

            }
        }

        public async Task DeleteCartAsync(int id)
        {
            try
            {
                await _cartRepository.DeleteCartAsync(id);
                _logger.LogInformation("Deleted cart ID {CartID}", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception when deleting cart ID {CartId}", id);
                //Handle concurrency exception
                throw new Exception("The cart was updated by another user. please reload and try again", ex);
            
            }
                
        }

        public async Task AddCartItemAsync(int customerId, CartItemDto cartItemdDto)
        {
            // Retrieve the cart for the specified customer
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            if (cart == null)
            {
                //If the cart does not exist, create a new cart
                cart = new Cart { CustomerId = customerId, CartItems = new List<CartItem>() };
                await _cartRepository.AddCartAsync(cart);
                _logger.LogInformation("Created new cart for customer ID {CustomerId}", customerId);
            }

            //Map the cartItemDto to a cartItem entity

            var cartItem = _mapper.Map<CartItem>(cartItemdDto);
            cartItem.CartId = cart.Id;

            //Add the new CartItem to the CartItems collection
            cart.CartItems.Add(cartItem);
            try {
                await _cartRepository.UpdateCartAsync(cart);
                _logger.LogInformation("Added item to cart ID {CartId} for customer ID {CustomerId}", cart.Id, customerId);


            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInformation("Added item to cart ID {CartId} for customer ID {CustomerId}", cart.Id, customerId);

                // Handle concurrency exception
                throw new Exception("The cart was updated by another user. Please reload and try again.", ex);
            }


        }
        
        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation("Getting cart items for customer ID {CustomerId}", customerId);

            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<CartItemDto>>(cart.CartItems);
        }
    }
}
