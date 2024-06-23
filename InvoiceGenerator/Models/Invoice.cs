using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace InvoiceGenerator.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string? InvoiceNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        public List<LineItem> LineItems { get; set; } = new List<LineItem>();

        [Column(TypeName = "decimal(18,2)")]
        public decimal VAT { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal => LineItems.Sum(li => li.Total);

        public decimal Total => Subtotal + VAT;

        public string? Notes { get; set; }

        public void CalculateVAT()
        {
            VAT = Subtotal * 0.19m; // Example VAT rate of 19%
        }
    }
}
