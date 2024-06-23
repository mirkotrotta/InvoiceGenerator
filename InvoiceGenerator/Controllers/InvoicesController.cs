using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using InvoiceGenerator.Data;
using InvoiceGenerator.Models;

namespace InvoiceGenerator.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public InvoicesController(ApplicationDbContext context, IConverter converter, ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _context = context;
            _converter = converter;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices.Include(i => i.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.LineItems)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,ClientId,LineItems")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                invoice.CalculateVAT(); // Calculate VAT based on line items
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", invoice.ClientId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.LineItems)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", invoice.ClientId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,ClientId,LineItems")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingInvoice = await _context.Invoices
                        .Include(i => i.LineItems)
                        .FirstOrDefaultAsync(i => i.Id == id);

                    if (existingInvoice != null)
                    {
                        existingInvoice.Date = invoice.Date;
                        existingInvoice.ClientId = invoice.ClientId;
                        existingInvoice.LineItems = invoice.LineItems;
                        existingInvoice.CalculateVAT(); // Calculate VAT based on line items

                        _context.Update(existingInvoice);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", invoice.ClientId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }

        // Generate PDF
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .Include(i => i.LineItems)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.CalculateVAT();

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = RenderRazorViewToString("Details", invoice),
                        WebSettings = { DefaultEncoding = "utf-8" },
                    }
                }
            };

            var pdf = _converter.Convert(doc);
            return File(pdf, "application/pdf", $"Invoice_{invoice.InvoiceNumber}.pdf");
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"The view '{viewName}' was not found. Searched locations: {string.Join(", ", viewResult.SearchedLocations)}");
                }
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext).Wait();
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
