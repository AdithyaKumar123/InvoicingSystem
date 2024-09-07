using InvoicingSystem.Models;

namespace InvoicingSystem.Repositories
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPurchasedProductRepository _purchasedProductRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, IPurchasedProductRepository purchasedProductRepository, ICustomerRepository customerRepository, IProductRepository productRepository)
        {
            _invoiceRepository = invoiceRepository;
            _purchasedProductRepository = purchasedProductRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task AddInvoiceWithProducts(Invoice invoice, List<PurchasedProduct> products)
        {
            try
            {
                _invoiceRepository.AddInvoice(invoice);
                _invoiceRepository.AddPurchasedProducts(products);

                // Subtract product quantities
                foreach (var product in products)
                {
                    // Get the product from the repository
                    var existingProduct = await _productRepository.GetProductByIdAsync(product.ProductId);

                    if (existingProduct != null)
                    {
                        // Subtract the quantity
                        existingProduct.Quantity -= product.Quantity;

                        // Update the product in the repository
                        await _productRepository.UpdateProductAsync(existingProduct);
                    }
                }
            }catch (Exception ex)
            {
                throw new InvalidOperationException("AddInvoiceWithProducts method error:", ex);
            }
        }

        public Invoice GetInvoiceById(Guid invoiceId)
        {
            try
            {
                // Get the invoice
                var invoice = _invoiceRepository.GetInvoiceById(invoiceId);

                if (invoice == null) return null;

                // Get the purchased products associated with the invoice
                var products = _purchasedProductRepository.GetPurchasedProductsByInvoiceId(invoiceId);

                // Create and return InvoiceDto with all necessary details
                return new Invoice
                {
                    Id = invoice.Id,
                    CustomerId = invoice.CustomerId,
                    Date = invoice.Date,
                    TotalPrice = invoice.TotalPrice,
                    Products = products.Select(p => new PurchasedProduct
                    {
                        Id = p.Id,
                        InvoiceId = p.InvoiceId,
                        ProductId = p.ProductId,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Discount = p.Discount
                    }).ToList()
                };
            }catch (Exception ex)
            {
                throw new InvalidOperationException("GetInvoiceById method error:", ex);
            }
        }

        public async Task<List<Invoice>>  GetAllInvoices()
        {
            try
            {
                var invoices = _invoiceRepository.GetAllInvoices();
                var result = new List<Invoice>();

                foreach (var invoice in invoices)
                {
                    var customer = await _customerRepository.GetCustomerByIdAsync(invoice.CustomerId);

                    var products = _purchasedProductRepository.GetPurchasedProductsByInvoiceId(invoice.Id);

                    var invoiceDto = new Invoice
                    {
                        Id = invoice.Id,
                        CustomerId = invoice.CustomerId,
                        CustomerName = customer.Name,
                        Date = invoice.Date,
                        TotalPrice = invoice.TotalPrice,
                        PaymentType = invoice.PaymentType,
                        Products = new List<PurchasedProduct>()
                    };
                    foreach (var product in products)
                    {
                        var productDetail = await _productRepository.GetProductByIdAsync(product.ProductId);

                        invoiceDto.Products.Add(new PurchasedProduct
                        {
                            Id = product.Id,
                            InvoiceId = product.InvoiceId,
                            ProductId = product.ProductId,
                            ProductName = productDetail?.Name, // Add ProductName here
                            Price = product.Price,
                            Quantity = product.Quantity,
                            Discount = product.Discount,
                            Tax = product.Tax
                        });
                    }

                    result.Add(invoiceDto);
                }

                return result;
            }catch(Exception ex) {

                throw new InvalidOperationException("GetAllInvoices method error:", ex);
            }
        }
    }
}
