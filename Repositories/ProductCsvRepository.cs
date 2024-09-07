using InvoicingSystem.Models;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
namespace InvoicingSystem.Repositories
{
    public class ProductCsvRepository : IProductRepository
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.csv");
        private readonly string _purchasedProductFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "purchased_products.csv");
        private List<Product> _products;
  

        public ProductCsvRepository()
        {
            _products = LoadProductsFromCsv();

        }

        private List<Product> LoadProductsFromCsv()
        {
            if (!File.Exists(_filePath))
                return new List<Product>();

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Product>().ToList();
        }

        private List<PurchasedProduct> LoadPurchasedProductsFromCsv()
        {
            if (!File.Exists(_purchasedProductFilePath))
                return new List<PurchasedProduct>();

            using var reader = new StreamReader(_purchasedProductFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<PurchasedProduct>().ToList();
        }

        private async Task SaveProductsToCsvAsync()
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(_products);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync() => await Task.FromResult(_products);

        public async Task<Product> GetProductByIdAsync(Guid id) => await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

        public async Task AddProductAsync(Product product)
        {
            // Assign a new GUID to the product
            product.Id = Guid.NewGuid();
            _products.Add(product);
            await SaveProductsToCsvAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await GetProductByIdAsync(product.Id);
            if (existingProduct == null) return;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Category = product.Category;
            await SaveProductsToCsvAsync();
        }

        public async Task<string> DeleteProductAsync(Guid id)
        {
            var product = await GetProductByIdAsync(id);
            if (product == null)
            {
                return "Product not found.";
            }

            // Check if the product has been purchased
            bool isPurchased = await IsProductPurchasedAsync(id);
            if (isPurchased)
            {
                return "Product cannot be deleted as it has already been purchased.";
            }

            _products.Remove(product);
            await SaveProductsToCsvAsync();
            return "Product deleted successfully....";
        }

        public async Task<bool> IsProductPurchasedAsync(Guid productId)
        {
            try
            {

                // Load the purchased products from CSV
                var purchasedProducts = LoadPurchasedProductsFromCsv();
                bool isPurchased = purchasedProducts.Any(pp => pp.ProductId == productId);
                return await Task.FromResult(isPurchased);
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
