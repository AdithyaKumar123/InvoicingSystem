using Microsoft.AspNetCore.Mvc;

namespace InvoicingSystem.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : Controller
    {
        [HttpGet("view")]
        public async Task<IActionResult> View()
        {
            try
            {
                // If you need to pass data to the view, fetch it here
                return View("~/Pages/Cart.cshtml");
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving the Product Page.");
            }
        }
    }
}
