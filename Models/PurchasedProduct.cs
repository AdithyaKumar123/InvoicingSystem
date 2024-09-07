namespace InvoicingSystem.Models
{
    public class PurchasedProduct
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
       
    }
}
