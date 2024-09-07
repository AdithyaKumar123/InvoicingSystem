using InvoicingSystem.Models;
using InvoicingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace InvoicingSystem.Controllers
{
    [ApiController]
    [Route("api/invoice")]
    public class InvoiceController : Controller
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        [HttpGet("view")]
        public async Task<IActionResult> View()
        {
            try
            {
                // If you need to pass data to the view, fetch it here
                return View("~/Pages/Invoices.cshtml");
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving the Product Page.");
            }
        }

        [HttpPost("createinvoice")]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice requestBody)
        {
            try
            {
                // Extract and validate data from the dynamic object
                var invoiceId = requestBody.Id;
                var customerId = requestBody.CustomerId;
                var date = (DateTime)requestBody.Date;
                var totalPrice = (decimal)requestBody.TotalPrice;

                var invoice = new Invoice
                {
                    Id = invoiceId,
                    CustomerId = customerId,
                    Date = date,
                    TotalPrice = totalPrice
                };

                var products = new List<PurchasedProduct>();

                // Check if 'Products' property exists
                if (requestBody.Products != null)
                {
                    foreach (var product in requestBody.Products)
                    {
                        var purchasedProduct = new PurchasedProduct
                        {
                            Id = product.Id,
                            InvoiceId = invoice.Id,
                            ProductId = product.ProductId,
                            Price = (decimal)product.Price,
                            Quantity = (int)product.Quantity,
                            Discount = (decimal)product.Discount
                        };
                        products.Add(purchasedProduct);
                    }
                }

                await _invoiceService.AddInvoiceWithProducts(invoice, products);

                return Ok("Invoice created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetInvoiceById(Guid id)
        {
            var invoice = _invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }
            return Ok(invoice);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices = await _invoiceService.GetAllInvoices();
            return Ok(invoices);
        }
    }
}
