using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models;

namespace ProyectoFinalDiseño.Controllers
{
    public class ContenidoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContenidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR CONTENIDOS
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contenido.ToListAsync());
        }

        // DETALLES DE UN CONTENIDO
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();

            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null) return NotFound();

            return View(contenido);
        }

        // FORMULARIO CREAR CONTENIDO
        public IActionResult Crear()
        {
            return View();
        }

        // PROCESAR CREACIÓN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Contenido contenido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contenido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contenido);
        }

        // FORMULARIO EDITAR
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null) return NotFound();

            return View(contenido);
        }

        // PROCESAR EDICIÓN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Contenido contenido)
        {
            if (id != contenido.ContenidoID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contenido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContenidoExists(contenido.ContenidoID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contenido);
        }

        // FORMULARIO ELIMINAR
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null) return NotFound();

            return View(contenido);
        }

        // PROCESAR ELIMINACIÓN
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido != null)
            {
                _context.Contenido.Remove(contenido);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // HABILITAR CONTENIDO
        public async Task<IActionResult> Habilitar(int id)
        {
            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null) return NotFound();

            contenido.Habilitado = true; // Corregido
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DESHABILITAR CONTENIDO
        public async Task<IActionResult> Deshabilitar(int id)
        {
            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null) return NotFound();

            contenido.Habilitado = false; // Corregido
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContenidoExists(int id)
        {
            return _context.Contenido.Any(e => e.ContenidoID == id); // Corregido
        }
    }
}

