using Microsoft.AspNetCore.Mvc;
using Project_Api.Dtos;
using Project_Api.Interfaces;
using Project_Api.Models;
using Project_Api.Utilities;

namespace Project_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                ProductImage = productDto.ProductImage?.FileName
            };

            await _productService.AddProductAsync(product);

            if (productDto.ProductImage != null)
            {
                var imagePath = ImageHelper.SaveImage(productDto.ProductImage, "ProductImages");
                product.ProductImage = imagePath;
                await _productService.UpdateProductAsync(product, productDto.ProductImage);
            }

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] ProductDto productDto)
        {
            var product = new Product
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                ProductImage = productDto.ProductImage?.FileName
            };

            if (productDto.ProductImage != null)
            {
                var imagePath = ImageHelper.SaveImage(productDto.ProductImage, "ProductImages");
                product.ProductImage = imagePath;
            }

            await _productService.UpdateProductAsync(product, productDto.ProductImage);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpGet("images/{folderName}/{imageName}")]
        public IActionResult GetImage(string folderName, string imageName)
        {
            var imagePath = ImageHelper.GetImagePath(folderName, imageName);
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var image = System.IO.File.OpenRead(imagePath);
            return File(image, "image/jpeg");
        }
    }
}
