using InvoicingSystem.Models;

namespace InvoicingSystem.Repositories
{
    public class CsvInvoiceRepository : IInvoiceRepository, IPurchasedProductRepository
    {
        private readonly string _invoiceFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "invoices.csv");
        private readonly string _purchasedProductsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "purchased_products.csv");

        public void AddInvoice(Invoice invoice)
        {
            try
            {
                var line = $"{invoice.Id},{invoice.CustomerId},{invoice.Date.ToString("yyyy-MM-dd")},{invoice.TotalPrice}, {invoice.PaymentType}";
                File.AppendAllLines(_invoiceFilePath, new[] { line });
            }catch (Exception ex)
            {
                throw new InvalidOperationException("AddInvoice method error:", ex);
            }
        }

        public void AddPurchasedProducts(List<PurchasedProduct> products)
        {
            foreach (var product in products)
            {
                AddPurchasedProduct(product);
            }
        }

        public void AddPurchasedProduct(PurchasedProduct product)
        {
            try
            {
                var line = $"{product.Id},{product.InvoiceId},{product.ProductId},{product.Price},{product.Quantity},{product.Discount},{product.Tax}, ''";
                File.AppendAllLines(_purchasedProductsFilePath, new[] { line });
            }catch  (Exception ex)
            {
                throw new InvalidOperationException("AddPurchasedProduct method error:", ex);
            }
        }


        public Invoice GetInvoiceById(Guid invoiceId)
        {
            try
            {
                var lines = File.ReadAllLines(_invoiceFilePath);
                foreach (var line in lines)
                {
                    var columns = line.Split(',');
                    if (Guid.Parse(columns[0]) == invoiceId)
                    {
                        return new Invoice
                        {
                            Id = Guid.Parse(columns[0]),
                            CustomerId = Guid.Parse(columns[1]),
                            Date = DateTime.Parse(columns[2]),
                            TotalPrice = decimal.Parse(columns[3])
                        };
                    }
                }
                return null;
            }catch  (Exception ex)
            {
                throw new InvalidOperationException("GetInvoiceById method error:", ex);
            }
        }


        public List<PurchasedProduct> GetPurchasedProductsByInvoiceId(Guid invoiceId)
        {
            try
            {
                var products = new List<PurchasedProduct>();
                var lines = File.ReadAllLines(_purchasedProductsFilePath);
                foreach (var line in lines.Skip(1))
                {
                    var columns = line.Split(',');
                    if (Guid.Parse(columns[1]) == invoiceId)
                    {
                        products.Add(new PurchasedProduct
                        {
                            Id = Guid.Parse(columns[0]),
                            InvoiceId = Guid.Parse(columns[1]),
                            ProductId = Guid.Parse(columns[2]),
                            Price = decimal.Parse(columns[3]),
                            Quantity = int.Parse(columns[4]),
                            Discount = decimal.Parse(columns[5]),
                            Tax = decimal.Parse(columns[6])
                        });
                    }
                }
                return products;
            }catch(Exception ex)
            {
                throw new InvalidOperationException("GetPurchasedProductsByInvoiceId method error:", ex);
            }
        }

        public List<Invoice> GetAllInvoices()
        {
            try
            {
                var invoices = new List<Invoice>();
                var lines = File.ReadAllLines(_invoiceFilePath);
                foreach (var line in lines.Skip(1))
                {
                    var columns = line.Split(',');
                    invoices.Add(new Invoice
                    {
                        Id = Guid.Parse(columns[0]),
                        CustomerId = Guid.Parse(columns[1]),
                        Date = DateTime.Parse(columns[2]),
                        TotalPrice = decimal.Parse(columns[3]),
                        PaymentType = columns[4],
                    });
                }
                return invoices;
            }catch (Exception ex)
            {
                throw new InvalidOperationException("GetAllInvoices method error:", ex);
            }
        }
    }
}
