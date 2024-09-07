using System.ComponentModel.DataAnnotations;

namespace InvoicingSystem.Models
{
    public class Customer
    {
        public Guid Id { get; set; } // Unique identifier for each customer

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; }

        [StringLength(300, ErrorMessage = "Address cannot be longer than 300 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid contact number format.")]
        [StringLength(15, ErrorMessage = "Contact number cannot be longer than 15 characters.")]
        public string ContactNumber { get; set; }
    }
}
