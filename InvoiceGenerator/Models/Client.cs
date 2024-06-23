using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        public string? Email { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
