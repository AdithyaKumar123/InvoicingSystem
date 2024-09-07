using InvoicingSystem.Models;

namespace InvoicingSystem.Repositories
{
    public interface IPurchasedProductRepository
    {
        void AddPurchasedProduct(PurchasedProduct product);

        List<PurchasedProduct> GetPurchasedProductsByInvoiceId(Guid invoiceId);
    }
}
