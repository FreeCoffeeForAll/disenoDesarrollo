using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models;


using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoFinalDiseño.Models.user;
using ProyectoFinalDiseño.Models.invoice;

using ProyectoFinalDiseño.Services;



namespace ProyectoFinalDiseño.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User_Application> _userManager;
        private readonly IEmailSender _emailSender;

        public InvoiceController(ApplicationDbContext context, UserManager<User_Application> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        //------------------------------------------------------------- GET: Invoice
        public async Task<IActionResult> Index(string searchUser, string status)
        {
            var invoicesQuery = _context.Invoices.Include(i => i.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchUser))
            {
                invoicesQuery = invoicesQuery.Where(i => i.User.UserName.Contains(searchUser));
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Paid")
                    invoicesQuery = invoicesQuery.Where(i => i.IsPaid);
                else if (status == "Pending")
                    invoicesQuery = invoicesQuery.Where(i => !i.IsPaid);
            }

            var invoices = await invoicesQuery.OrderByDescending(i => i.BillingDate).ToListAsync();

            return View(invoices);
        }

        //------------------------------------------------------------- GET: Invoice/Export
        public async Task<IActionResult> ExportToExcel()
        {

            var invoices = await _context.Invoices
                .Include(i => i.User)
                .OrderByDescending(i => i.BillingDate)
                .ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Invoices");

            // Headers
            ws.Cells[1, 1].Value = "Invoice ID";
            ws.Cells[1, 2].Value = "User";
            ws.Cells[1, 3].Value = "Billing Date";
            ws.Cells[1, 4].Value = "Period Start";
            ws.Cells[1, 5].Value = "Period End";
            ws.Cells[1, 6].Value = "Amount";
            ws.Cells[1, 7].Value = "Status";

            using (var range = ws.Cells[1, 1, 1, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Rows
            int row = 2;
            foreach (var inv in invoices)
            {
                ws.Cells[row, 1].Value = inv.Id;
                ws.Cells[row, 2].Value = inv.User?.UserName;
                ws.Cells[row, 3].Value = inv.BillingDate.ToString("yyyy-MM-dd");
                ws.Cells[row, 4].Value = inv.PeriodStart.ToShortDateString();
                ws.Cells[row, 5].Value = inv.PeriodEnd.ToShortDateString();
                ws.Cells[row, 6].Value = inv.Amount;
                ws.Cells[row, 7].Value = inv.IsPaid ? "Paid" : "Pending";
                row++;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"Invoices_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        //------------------------------------------------------------- GET: Invoice/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.Invoices
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null) return NotFound();

            return View(invoice);
        }

        //------------------------------------------------------------- GET: Invoice/Create
        public IActionResult Create()
        {
            return View();
        }

        //------------------------------------------------------------- POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,BillingDate,PeriodStart,PeriodEnd,Amount,IsPaid")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        //------------------------------------------------------------- POST: Invoice/Mark as paid
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.IsPaid = true;
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Invoice #{invoice.Id} marked as Paid.";
            return RedirectToAction(nameof(Index));
        }

        //------------------------------------------------------------- GET: Invoice/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Email", invoice.UserId);
            return View(invoice);
        }

        //------------------------------------------------------------- POST: Invoice/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,BillingDate,PeriodStart,PeriodEnd,Amount,IsPaid")] Invoice invoice)
        {
            if (id != invoice.Id) return NotFound();

            // Retrieve the existing invoice to preserve userId and check if it exists
            var existingInvoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id);

            if (existingInvoice == null)
            {
                return NotFound();
            }

            // Preserve the UserId from the existing invoice (do not overwrite it)
            invoice.UserId = existingInvoice.UserId;
            
            if (ModelState.IsValid)
            {
                try
                {
                    // Detach the existing entity to avoid tracking conflicts
                    _context.Entry(existingInvoice).State = EntityState.Detached;

                    // Update the new entity
                    _context.Entry(invoice).State = EntityState.Modified;

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    TempData["Message"] = "Invoice updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Invoices.Any(e => e.Id == invoice.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error en '{key}': {error.ErrorMessage}");
                    }
                }

                ViewBag.Users = new SelectList(_context.Users.ToList(), "Id", "Email", invoice.UserId);
                return View(invoice);
            }

            return View(invoice);
        }

        //------------------------------------------------------------- GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var invoice = await _context.Invoices
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null) return NotFound();

            return View(invoice);
        }

        //------------------------------------------------------------- POST: Invoice/Delete/5
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

            TempData["Message"] = "Invoice deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        //------------------------------------------------------------- GET: Invoicing Page
        public IActionResult GenerateInvoices()
        {
            return View();
        }

        //------------------------------------------------------------- POST: Trigger Invoice Generation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateInvoicesConfirmed()
        {
            var today = DateTime.Today;
            var periodStart = new DateTime(today.Year, today.Month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);

            var activeSubscriptions = _context.Subscriptions
                .Where(s => s.EndDate == null || s.EndDate > DateTime.Now)
                .ToList();

            foreach (var subscription in activeSubscriptions)
            {
                var alreadyExists = _context.Invoices.Any(i =>
                    i.UserId == subscription.UserId &&
                    i.PeriodStart == periodStart);

                if (!alreadyExists)
                {
                    var invoice = new Invoice
                    {
                        UserId = subscription.UserId,
                        BillingDate = DateTime.Now,
                        PeriodStart = periodStart,
                        PeriodEnd = periodEnd,
                        Amount = subscription.Price
                    };
                    _context.Invoices.Add(invoice);
                    // Envía el correo
                    var user = await _userManager.FindByIdAsync(subscription.UserId);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        string subject = $"Your Monthly Invoice - {periodStart:MMMM yyyy}";
                        string htmlMessage = $@"
                <p>Dear {user.Name},</p>
                <p>Thank you for your subscription. Here are your invoice details:</p>
                <ul>
                    <li><strong>Invoice Date:</strong> {invoice.BillingDate:yyyy-MM-dd}</li>
                    <li><strong>Period:</strong> {invoice.PeriodStart:yyyy-MM-dd} to {invoice.PeriodEnd:yyyy-MM-dd}</li>
                    <li><strong>Amount:</strong> ₡{invoice.Amount:N2}</li>
                </ul>
                <p>Please make your payment at your earliest convenience.</p>
                <p>Best regards,<br/>Move App</p>";

                        await _emailSender.SendEmailAsync(user.Email, subject, htmlMessage);
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Invoices generated successfully.";
            return RedirectToAction("GenerateInvoices");
        }
    }
}
