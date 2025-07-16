using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models;
using ProyectoFinalDiseño.Models.Training;
using System.Security.Claims;

namespace ProyectoFinalDiseño.Controllers
{
    [Authorize(Roles = "Admin,Trainer,Client")]
    public class ClassReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //------------------------------------------------------------- GET: ClassReservations (Visualización semanal)
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = await _context.ClassReservations
                .Include(r => r.TrainingClass)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(reservations);
        }

        //------------------------------------------------------------- GET: ClassReservations/Create
        public async Task<IActionResult> Create()
        {
            // Mostrar solo clases con cupo y de la semana actual
            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);

            var availableClasses = await _context.TrainingClasses
                .Where(c => c.CurrentParticipants < c.MaxParticipants)
                .ToListAsync();

            return View(availableClasses);
        }

        //------------------------------------------------------------- POST: ClassReservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int trainingClassId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var trainingClass = await _context.TrainingClasses.FindAsync(trainingClassId);
            if (trainingClass == null || trainingClass.CurrentParticipants >= trainingClass.MaxParticipants)
            {
                return NotFound("Class not available");
            }

            var reservation = new ClassReservation
            {
                TrainingClassId = trainingClassId,
                UserId = userId
            };

            _context.ClassReservations.Add(reservation);
            trainingClass.CurrentParticipants += 1;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //------------------------------------------------------------- POST: ClassReservations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.ClassReservations
                .Include(r => r.TrainingClass)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation != null)
            {
                reservation.TrainingClass.CurrentParticipants -= 1;
                _context.ClassReservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
