using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project_Api.Dtos;
using Project_Api.Interfaces;
using Project_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_Api.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, IMapper mapper, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CartDto> GetCartByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation("Getting cart for customer ID {CustomerId}", customerId);
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task AddCartAsync(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            await _cartRepository.AddCartAsync(cart);
            _logger.LogInformation("Added cart for customer ID {CustomerId}", cartDto.CustomerId);
        }

        public async Task UpdateCartAsync(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            try
            {
                await _cartRepository.UpdateCartAsync(cart);
                _logger.LogInformation("Updated cart ID {CartId}", cartDto.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception when updating cart ID {CartId}", cartDto.Id);
                throw new Exception("The cart was updated by another user. Please reload and try again.", ex);
            }
        }

        public async Task DeleteCartAsync(int id)
        {
            try
            {
                await _cartRepository.DeleteCartAsync(id);
                _logger.LogInformation("Deleted cart ID {CartId}", id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception when deleting cart ID {CartId}", id);
                throw new Exception("The cart was updated by another user. Please reload and try again.", ex);
            }
        }

        public async Task AddCartItemAsync(int customerId, CartItemDto cartItemDto)
        {
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            if (cart == null)
            {
                cart = new Cart { CustomerId = customerId, CartItems = new List<CartItem>() };
                await _cartRepository.AddCartAsync(cart);
                _logger.LogInformation("Created new cart for customer ID {CustomerId}", customerId);
            }

            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            cartItem.CartId = cart.Id;
            cart.CartItems.Add(cartItem);

            try
            {
                await _cartRepository.UpdateCartAsync(cart);
                _logger.LogInformation("Added item to cart ID {CartId} for customer ID {CustomerId}", cart.Id, customerId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception when adding item to cart ID {CartId}", cart.Id);
                throw new Exception("The cart was updated by another user. Please reload and try again.", ex);
            }
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCustomerIdAsync(int customerId)
        {
            _logger.LogInformation("Getting cart items for customer ID {CustomerId}", customerId);
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<CartItemDto>>(cart.CartItems);
        }

        public async Task UpdateCartItemAsync(int customerId, CartItemDto cartItemDto)
        {
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            if (cart == null)
            {
                _logger.LogWarning("Cart not found for customer ID {CustomerId}", customerId);
                throw new KeyNotFoundException("Cart not found");

            }

            var cartItem = cart.CartItems.FirstOrDefault(ci=>ci.Id == cartItemDto.Id);
            if (cartItem == null)
            { 
                _logger.LogWarning("Cart item not found for customer ID {CustomerId} and item ID {ItemId}", customerId, cartItemDto.Id);
                throw new KeyNotFoundException("Cart item not found");
            }
            cartItem.ProductId = cartItemDto.ProductId;
            cartItem.Quantity = cartItemDto.Quantity;
            cartItem.Price = cartItemDto.Price;

            try { 
                await _cartRepository.UpdateCartAsync(cart);
                _logger.LogInformation("Updated cart item ID {ItemId} for customer ID {CustomerId}", cartItemDto.Id, customerId);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency exception when updating cart item ID {ItemId} for customer ID {CustomerId}", cartItemDto.Id, customerId);
                throw new Exception("The cart was updated by another user. Please reload and try again.", ex);
            }

        }
    }
}

