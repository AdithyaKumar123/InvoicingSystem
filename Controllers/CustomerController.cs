using InvoicingSystem.Models;
using InvoicingSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InvoicingSystem.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public CustomerController(ICustomerRepository customerRepository, IInvoiceRepository invoiceRepository)
        {
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet("view")]
        public async Task<IActionResult> View()
        {
            try
            {
                // If you need to pass data to the view, fetch it here
                return View("~/Pages/Customers.cshtml");
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving the Customer Page.");
            }
        }

        // GET: api/customer
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAllCustomersAsync();
                return Ok(customers);
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/customer/addcustomer
        [HttpPost("addcustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                customer.Id = Guid.NewGuid();
                await _customerRepository.AddCustomerAsync(customer);
                return Ok(customer);
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/customer/updatecustomer
        [HttpPut("updatecustomer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] Customer customer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingCustomer = await _customerRepository.GetCustomerByIdAsync(customer.Id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                await _customerRepository.UpdateCustomerAsync(customer);
                return Ok(customer);
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/customer/deletecustomer/{id}
        [HttpDelete("deletecustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {

                var existingCustomer = await _customerRepository.GetCustomerByIdAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                var invoices = _invoiceRepository.GetAllInvoices(); // Assuming you have a method to fetch all invoices
                bool isCustomerInUse = invoices.Any(invoice => invoice.CustomerId == id);

                if (isCustomerInUse)
                {
                    // Return a response indicating that the customer cannot be deleted
                    return BadRequest($"Cannot delete customer '{existingCustomer.Name}' because it is associated with existing invoices.");
                }
                await _customerRepository.DeleteCustomerAsync(id);
                return NoContent();
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    
}
}
