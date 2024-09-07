using InvoicingSystem.Models;


namespace InvoicingSystem.Repositories
{
    public interface IProductRepository
    {
            Task<IEnumerable<Product>> GetAllProductsAsync();
            Task<Product> GetProductByIdAsync(Guid id);
            Task AddProductAsync(Product product);
            Task UpdateProductAsync(Product product);
            Task<string> DeleteProductAsync(Guid id);
       
    }
}
