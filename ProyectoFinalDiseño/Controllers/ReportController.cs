using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDiseño.Models;
using System;
using System.Linq;
using System.Text.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;


namespace ProyectoFinalDiseño.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult VisitChart()
        {
            // Step 1: Get grouped data from database (no .ToString formatting)
            var visitsRaw = _context.UserVisits
                .GroupBy(v => v.VisitTime.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToList(); // Now it's LINQ-to-Objects

            // Step 2: Format dates and prepare for chart
            var visits = visitsRaw
                .Select(v => new { Date = v.Date.ToString("yyyy-MM-dd"), v.Count })
                .OrderBy(v => v.Date)
                .ToList();

            ViewBag.VisitDataJson = JsonSerializer.Serialize(visits); // data sent

            return View();
        }


        [HttpPost]
        public IActionResult ExportVisitsToExcel() 
        {
            var visitsRaw = _context.UserVisits
                .GroupBy(v => v.VisitTime.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderBy(g => g.Date)
                .ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Daily Visits");

            //Headers
            worksheet.Cells[1, 1].Value = "Date";
            worksheet.Cells[1, 2].Value = "Visit Count";
            worksheet.Row(1).Style.Font.Bold = true;

            //Data
            for (int i = 0; i < visitsRaw.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = visitsRaw[i].Date.ToString("yyyy-MM-dd");
                worksheet.Cells[i + 2, 2].Value = visitsRaw[i].Count;
            }
                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Daily_Visits>{DateTime.Now:yyyyMMddHHmmss}.xlsx";


                return File(stream,
                    "application/vnd.opencmlformats-officedocument.spredsheetml.sheet",
                    excelName
                    );
            
        }
    }
}
