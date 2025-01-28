using Project_Api.Models;

namespace Project_Api.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<string> AddOrderAsync(Order order);
        Task<string> UpdateOrderAsync(Order order);
        Task<string> DeleteOrderAsync(int id);
    }
}
