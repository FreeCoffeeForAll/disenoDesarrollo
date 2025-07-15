using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models;
using ProyectoFinalDiseño.Models.Training;

namespace ProyectoFinalDiseño.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainingClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //------------------------------------------------------------- GET: TrainingClasses
        public async Task<IActionResult> Index()
        {
            var classes = await _context.TrainingClasses.ToListAsync();
            return View(classes);
        }

        //------------------------------------------------------------- GET: TrainingClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trainingClass = await _context.TrainingClasses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingClass == null) return NotFound();

            return View(trainingClass);
        }

        //------------------------------------------------------------- GET: TrainingClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        //------------------------------------------------------------- POST: TrainingClasses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Parse TimeSpan desde string
                TimeSpan startTime;
                TimeSpan endTime;

                if (!TimeSpan.TryParse(model.StartTime, out startTime))
                {
                    ModelState.AddModelError("StartTime", "Invalid start time format.");
                    return View(model);
                }

                if (!TimeSpan.TryParse(model.EndTime, out endTime))
                {
                    ModelState.AddModelError("EndTime", "Invalid end time format.");
                    return View(model);
                }

                var trainingClass = new TrainingClass
                {
                    Title = model.Title,
                    Description = model.Description,
                    DayOfWeek = model.DayOfWeek,
                    StartTime = startTime,
                    EndTime = endTime,
                    MaxParticipants = model.MaxParticipants
                };

                _context.Add(trainingClass);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Class created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        //------------------------------------------------------------- GET: TrainingClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var trainingClass = await _context.TrainingClasses.FindAsync(id);
            if (trainingClass == null) return NotFound();

            return View(trainingClass);
        }

        //------------------------------------------------------------- POST: TrainingClasses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DayOfWeek,StartTime,EndTime,MaxParticipants,CurrentParticipants")] TrainingClass trainingClass)
        {
            if (id != trainingClass.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TrainingClasses.Any(e => e.Id == trainingClass.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainingClass);
        }

        //------------------------------------------------------------- GET: TrainingClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var trainingClass = await _context.TrainingClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingClass == null) return NotFound();

            return View(trainingClass);
        }

        //------------------------------------------------------------- POST: TrainingClasses/Delete/5
        [HttpPost] //, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("TrainingClasses/DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingClass = await _context.TrainingClasses.FindAsync(id);
            if (trainingClass != null)
            {
                _context.TrainingClasses.Remove(trainingClass);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Class deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
