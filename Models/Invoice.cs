namespace InvoicingSystem.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; } // Add CustomerName
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public string  PaymentType { get; set; }

        public List<PurchasedProduct> Products { get; set; }
    }
}
