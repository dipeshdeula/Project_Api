using Project_Api.Dtos;

namespace Project_Api.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartByCustomerIdAsync(int customerId);
        Task AddCartAsync(CartDto cartDto);
        Task UpdateCartAsync(CartDto cartDto);
        Task DeleteCartAsync(int id);
        Task AddCartItemAsync(int customerId, CartItemDto cartItemDto);
        Task UpdateCartItemAsync (int customerId, CartItemDto cartItemDto);
        Task<IEnumerable<CartItemDto>> GetCartItemsByCustomerIdAsync(int customerId);



    }
}
