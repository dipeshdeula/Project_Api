using Microsoft.EntityFrameworkCore;
using Project_Api.Data;
using Project_Api.Interfaces;
using Project_Api.Models;

namespace Project_Api.Repositries
{
    public class ProductRepo : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");

            }
            return product;
        }
        public async Task<string> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return "Product Added Successfully";
        }

        public async Task<string> UpdateProductAsync(Product product)
        {
            var productDetail = await _context.Products.FindAsync(product.Id);
            if (productDetail == null)
            {
                return "Product not found";
            }
            productDetail.Name = product.Name;
            productDetail.Description = product.Description;
            productDetail.ProductImage = product.ProductImage;
            await _context.SaveChangesAsync();
            return "Product Updated Successfully";
        }

        public async Task<string> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return "Product not found";
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return "Product Deleted Successfully";
        }


    }
}
