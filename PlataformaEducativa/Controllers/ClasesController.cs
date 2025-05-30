using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaEducativa.Data;
using PlataformaEducativa.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEducativa.Controllers
{
    public class ClasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clases
        public async Task<IActionResult> Index(int? subtemaId)
        {
            if (subtemaId == null)
            {
                return NotFound();
            }

            var subtema = await _context.Subtemas
                .Include(s => s.Clases)
                .FirstOrDefaultAsync(s => s.SubtemaId == subtemaId);

            if (subtema == null)
            {
                return NotFound();
            }

            return View(subtema);
        }

        // GET: Clases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases
                .Include(c => c.Subtema)
                .Include(c => c.Archivos)
                .FirstOrDefaultAsync(m => m.ClaseId == id);

            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // GET: Clases/Create
        public IActionResult Create(int subtemaId)
        {
            ViewBag.SubtemaId = subtemaId;
            return View();
        }

        // POST: Clases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaseId,SubtemaId,Nombre,Descripcion,Orden")] Clase clase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { subtemaId = clase.SubtemaId });
            }
            ViewBag.SubtemaId = clase.SubtemaId;
            return View(clase);
        }

        // GET: Clases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases.FindAsync(id);
            if (clase == null)
            {
                return NotFound();
            }
            return View(clase);
        }

        // POST: Clases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaseId,SubtemaId,Nombre,Descripcion,Orden")] Clase clase)
        {
            if (id != clase.ClaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaseExists(clase.ClaseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { subtemaId = clase.SubtemaId });
            }
            return View(clase);
        }

        // GET: Clases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases
                .Include(c => c.Subtema)
                .FirstOrDefaultAsync(m => m.ClaseId == id);
            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // POST: Clases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clase = await _context.Clases.FindAsync(id);
            _context.Clases.Remove(clase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { subtemaId = clase.SubtemaId });
        }

        private bool ClaseExists(int id)
        {
            return _context.Clases.Any(e => e.ClaseId == id);
        }
    }
}