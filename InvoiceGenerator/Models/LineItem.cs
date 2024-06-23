using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Models
{
    public class LineItem
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public int InvoiceId { get; set; }

        public Invoice? Invoice { get; set; }

        public decimal Total => Quantity * UnitPrice;
    }
}
