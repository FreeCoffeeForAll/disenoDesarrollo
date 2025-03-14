﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalDiseño.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAR TODOS LOS OBJETOS EN INVENTARIO (READ)
        public async Task<IActionResult> Index()
        {
            var inventario = await _context.Inventario.Include(i => i.Categoria).ToListAsync();
            return View(inventario);
        }

        // DETALLES DE UN OBJETO EN INVENTARIO
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
                return NotFound();

            var inventario = await _context.Inventario.Include(i => i.Categoria)
                                .FirstOrDefaultAsync(m => m.ObjetoID == id);
            if (inventario == null)
                return NotFound();

            return View(inventario);
        }

        // MOSTRAR FORMULARIO DE CREACIÓN
        public IActionResult Crear()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaID", "NombreCategoria");
            return View();
        }

        // PROCESAR CREACIÓN (CREATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaID", "NombreCategoria", inventario.CategoriaID);
            return View(inventario);
        }

        // MOSTRAR FORMULARIO DE EDICIÓN
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
                return NotFound();

            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario == null)
                return NotFound();

            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaID", "NombreCategoria", inventario.CategoriaID);
            return View(inventario);
        }

        // PROCESAR EDICIÓN (UPDATE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Inventario inventario)
        {
            if (id != inventario.ObjetoID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.ObjetoID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "CategoriaID", "NombreCategoria", inventario.CategoriaID);
            return View(inventario);
        }

        // MOSTRAR FORMULARIO DE ELIMINACIÓN
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
                return NotFound();

            var inventario = await _context.Inventario.Include(i => i.Categoria)
                                .FirstOrDefaultAsync(m => m.ObjetoID == id);
            if (inventario == null)
                return NotFound();

            return View(inventario);
        }

        // PROCESAR ELIMINACIÓN (DELETE)
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario != null)
            {
                _context.Inventario.Remove(inventario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventario.Any(e => e.ObjetoID == id);
        }
    }
}

