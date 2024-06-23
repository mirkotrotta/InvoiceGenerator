using InvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
    }
}
