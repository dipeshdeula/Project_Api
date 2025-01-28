using Project_Api.Models;

namespace Project_Api.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
    }
}
