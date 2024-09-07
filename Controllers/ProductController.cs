using InvoicingSystem.Models;
using InvoicingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InvoicingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        // Action to render the HTML page for products
        [HttpGet("view")]
        public async Task<IActionResult> View()
        {
            try
            {
                // If you need to pass data to the view, fetch it here
                return View("~/Pages/Products.cshtml");
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving the Product Page.");
            }
        }

        // API endpoint to get all products in JSON format
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                return Ok(products);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving products.");
            }
        }


        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.AddProductAsync(product);
                    return Ok(new { success = true, message = "Product added successfully." });
                }
                catch
                {
                    return StatusCode(500, new { success = false, message = "An error occurred while adding the product." });
                }
            }

            return BadRequest(new { success = false, message = "Invalid product data." });
        }


        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateProductAsync(product);
                    return Ok(new { success = true, message = "Product updated successfully." });
                }
                catch
                {
                    return StatusCode(500, new { success = false, message = "An error occurred while updating the product." });
                }
            }

            return BadRequest(new { success = false, message = "Invalid product data." });
        }

        [HttpDelete("deleteproduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                 string response =   await _productRepository.DeleteProductAsync(id);
                return Ok(new { success = true, message = response });
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the product." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving the product.");
            }
        }
    }
}
