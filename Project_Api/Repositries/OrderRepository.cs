using Microsoft.EntityFrameworkCore;
using Project_Api.Data;
using Project_Api.Interfaces;
using Project_Api.Models;

namespace Project_Api.Repositries
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();

        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders.Include(o=>o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<string> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return "Order Added Successfully";
        }

        public async Task<string> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return "Order Updated Successfully";
        }

        public async Task<string> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order !=null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return "Order Deleted Successfully";
            }
            else
            {
                return "Order not found";

            }
        }       

       
    }
}
