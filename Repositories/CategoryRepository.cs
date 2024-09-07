using CsvHelper;
using InvoicingSystem.Models;
using System.Globalization;

namespace InvoicingSystem.Repositories
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "categories.csv");

        private readonly IProductRepository _productRepository;
        public CategoryRepository(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await Task.Run(() =>
                {
                    using var reader = new StreamReader(_filePath);
                    using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));
                    return csv.GetRecords<Category>().ToList();
                });
            }catch (Exception ex)
            {
                throw new InvalidOperationException("GetAllCategoriesAsync method error:", ex);
            }
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var categories = await GetAllCategoriesAsync();
                return categories.FirstOrDefault(c => c.Id == id);

            }catch(Exception ex)
            {
                throw new InvalidOperationException("GetCategoryByIdAsync method error:", ex);
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                var categories = (await GetAllCategoriesAsync()).ToList();
                categories.Add(category);
                await SaveCategoriesAsync(categories);
            }catch (Exception ex)
            {
                throw new InvalidOperationException("AddCategoryAsync method error:", ex);
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                var categories = (await GetAllCategoriesAsync()).ToList();
                var index = categories.FindIndex(c => c.Id == category.Id);
                if (index >= 0)
                {
                    categories[index] = category;
                    await SaveCategoriesAsync(categories);
                }
            }catch (Exception ex)
            {
                throw new InvalidOperationException("UpdateCategoryAsync method error:", ex);
            }
        }

        public async Task<string> DeleteCategoryAsync(Guid id)
        {
            try
            {
                var categories = (await GetAllCategoriesAsync()).ToList();
                var categoryToRemove = categories.FirstOrDefault(c => c.Id == id);
                if (categoryToRemove != null)
                {
                    // Check if the category is used
                    if (await IsCategoryInUseAsync(categoryToRemove.Name))
                    {
                        return "Cannot delete category  because it is already in use";

                    }
                    categories.Remove(categoryToRemove);
                    await SaveCategoriesAsync(categories);
                    return "Deleted successfully";
                }
                return "Not found";
            }catch  (Exception ex)
            {
                throw new InvalidOperationException("DeleteCategoryAsync method error:", ex);
            }
        }

        private async Task SaveCategoriesAsync(IEnumerable<Category> categories)
        {
            try
            {
                using var writer = new StreamWriter(_filePath);
                using var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));
                await csv.WriteRecordsAsync(categories);
            }catch  (Exception ex)
            {
                throw new InvalidOperationException("SaveCategoriesAsync method error:", ex);
            }
        }

        private async Task<bool> IsCategoryInUseAsync(string categoryName)
        {
            try
            {
                // Fetch all products or entities that might use this category
                var products = await _productRepository.GetAllProductsAsync();

                // Check if any product uses the category
                return products.Any(p => p.Category.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            }catch (Exception ex)
            {
                throw new InvalidOperationException("IsCategoryInUseAsync method error:", ex);
            }
        }
    }
}
