using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaEducativa.Data;
using PlataformaEducativa.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEducativa.Controllers
{
    public class SubtemasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubtemasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subtemas
        public async Task<IActionResult> Index(int? unidadId)
        {
            if (unidadId == null)
            {
                return NotFound();
            }

            var unidad = await _context.Unidades
                .Include(u => u.Subtemas)
                .FirstOrDefaultAsync(u => u.UnidadId == unidadId);

            if (unidad == null)
            {
                return NotFound();
            }

            return View(unidad);
        }

        // GET: Subtemas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtema = await _context.Subtemas
                .Include(s => s.Unidad)
                .Include(s => s.Clases)
                .FirstOrDefaultAsync(m => m.SubtemaId == id);

            if (subtema == null)
            {
                return NotFound();
            }

            return View(subtema);
        }

        // GET: Subtemas/Create
        public IActionResult Create(int unidadId)
        {
            ViewBag.UnidadId = unidadId;
            return View();
        }

        // POST: Subtemas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubtemaId,UnidadId,Nombre,Descripcion,Orden")] Subtema subtema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subtema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { unidadId = subtema.UnidadId });
            }
            ViewBag.UnidadId = subtema.UnidadId;
            return View(subtema);
        }

        // GET: Subtemas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtema = await _context.Subtemas.FindAsync(id);
            if (subtema == null)
            {
                return NotFound();
            }
            return View(subtema);
        }

        // POST: Subtemas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubtemaId,UnidadId,Nombre,Descripcion,Orden")] Subtema subtema)
        {
            if (id != subtema.SubtemaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subtema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubtemaExists(subtema.SubtemaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { unidadId = subtema.UnidadId });
            }
            return View(subtema);
        }

        // GET: Subtemas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtema = await _context.Subtemas
                .Include(s => s.Unidad)
                .FirstOrDefaultAsync(m => m.SubtemaId == id);
            if (subtema == null)
            {
                return NotFound();
            }

            return View(subtema);
        }

        // POST: Subtemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subtema = await _context.Subtemas.FindAsync(id);
            _context.Subtemas.Remove(subtema);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { unidadId = subtema.UnidadId });
        }

        private bool SubtemaExists(int id)
        {
            return _context.Subtemas.Any(e => e.SubtemaId == id);
        }
    }
}