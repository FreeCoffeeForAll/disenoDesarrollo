using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalDiseño.Models;


namespace ProyectoFinalDiseño.Controllers
{
    public class InventarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inventario
        public async Task<IActionResult> Index()
        {
            var inventario = await _context.Inventario.Include(i => i.Categoria).ToListAsync();
            return View(inventario);
        }

        // GET: Inventario/Create
        public IActionResult Create()
        {
            return View();
        }

        public ApplicationDbContext Get_context()
        {
            return _context;
        }

        // POST: Inventario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Cantidad,Estado,CategoriaID")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventario);
        }
    }
}
