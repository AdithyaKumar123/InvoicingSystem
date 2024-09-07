using InvoicingSystem.Models;

namespace InvoicingSystem.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task<string> DeleteCategoryAsync(Guid id);
    }
}
