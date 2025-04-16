using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDiseño.Models;
using System;
using System.Linq;
using System.Text.Json;

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

    }
}
