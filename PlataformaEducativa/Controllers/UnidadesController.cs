using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaEducativa.Data;
using PlataformaEducativa.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEducativa.Controllers
{
    public class UnidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Unidades
        public async Task<IActionResult> Index(int? materiaId)
        {
            if (materiaId == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .Include(m => m.Unidades)
                .FirstOrDefaultAsync(m => m.MateriaId == materiaId);

            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // GET: Unidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidades
                .Include(u => u.Materia)
                .Include(u => u.Subtemas)
                .FirstOrDefaultAsync(m => m.UnidadId == id);

            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        // GET: Unidades/Create
        public IActionResult Create(int materiaId)
        {
            ViewBag.MateriaId = materiaId;
            return View();
        }

        // POST: Unidades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UnidadId,MateriaId,Nombre,Descripcion,Orden")] Unidad unidad)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unidad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { materiaId = unidad.MateriaId });
            }
            ViewBag.MateriaId = unidad.MateriaId;
            return View(unidad);
        }

        // GET: Unidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidades.FindAsync(id);
            if (unidad == null)
            {
                return NotFound();
            }
            return View(unidad);
        }

        // POST: Unidades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UnidadId,MateriaId,Nombre,Descripcion,Orden")] Unidad unidad)
        {
            if (id != unidad.UnidadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnidadExists(unidad.UnidadId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { materiaId = unidad.MateriaId });
            }
            return View(unidad);
        }

        // GET: Unidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidades
                .Include(u => u.Materia)
                .FirstOrDefaultAsync(m => m.UnidadId == id);
            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        // POST: Unidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unidad = await _context.Unidades.FindAsync(id);
            _context.Unidades.Remove(unidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { materiaId = unidad.MateriaId });
        }

        private bool UnidadExists(int id)
        {
            return _context.Unidades.Any(e => e.UnidadId == id);
        }
    }
}