using InvoicingSystem.Models;
using InvoicingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InvoicingSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("view")]
        public async Task<IActionResult> View()
        {
            try
            {
                // If you need to pass data to the view, fetch it here
                return View("~/Pages/Categories.cshtml");
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving the Product Page.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _categoryRepository.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory( [FromBody] Category category)
        {
            try
            {
                await _categoryRepository.UpdateCategoryAsync(category);
                return Ok(new { success = true, message = "Product updated successfully." });
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating the product." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
           string response =  await _categoryRepository.DeleteCategoryAsync(id);
            return Ok(new { success = true, message = response });
        }
    }
}
