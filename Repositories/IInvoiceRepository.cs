using InvoicingSystem.Models;

namespace InvoicingSystem.Repositories
{
    public interface IInvoiceRepository
    {
        void AddInvoice(Invoice invoice);
        void AddPurchasedProducts(List<PurchasedProduct> products);

        Invoice GetInvoiceById(Guid invoiceId);  // Retrieve an invoice by its ID
        List<Invoice> GetAllInvoices();  // Retrieve all invoices
    }
}
