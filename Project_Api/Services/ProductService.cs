using Project_Api.Interfaces;
using Project_Api.Models;
using Project_Api.Utilities;

namespace Project_Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
            return product;
        }

        public async Task UpdateProductAsync(Product product, IFormFile? productImage = null)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(product.Id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            existingProduct.Name = product.Name ?? existingProduct.Name;
            existingProduct.Description = product.Description ?? existingProduct.Description;

            if (productImage != null)
            {
                var imagePath = ImageHelper.SaveImage(productImage, "ProductImages");
                existingProduct.ProductImage = imagePath;
            }

            await _productRepository.UpdateProductAsync(existingProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                ImageHelper.DeleteImage("ProductImages", product.ProductImage);
            }

            await _productRepository.DeleteProductAsync(id);
        }
    }
}
