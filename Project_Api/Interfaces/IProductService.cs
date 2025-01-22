using Project_Api.Models;

namespace Project_Api.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task UpdateProductAsync(Product product, IFormFile? productImage=null);
        Task DeleteProductAsync(int id);
    }
}
