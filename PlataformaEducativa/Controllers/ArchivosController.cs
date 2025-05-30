using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaEducativa.Data;
using PlataformaEducativa.Models;
using PlataformaEducativa.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEducativa.Controllers
{
    public class ArchivosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGoogleDriveService _driveService;

        public ArchivosController(ApplicationDbContext context, IGoogleDriveService driveService)
        {
            _context = context;
            _driveService = driveService;
        }

        // GET: Archivos
        public async Task<IActionResult> Index(int? claseId)
        {
            if (claseId == null)
            {
                return NotFound();
            }

            var clase = await _context.Clases
                .Include(c => c.Archivos)
                .FirstOrDefaultAsync(c => c.ClaseId == claseId);

            if (clase == null)
            {
                return NotFound();
            }

            return View(clase);
        }

        // GET: Archivos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archivo = await _context.Archivos
                .Include(a => a.Clase)
                .FirstOrDefaultAsync(m => m.ArchivoId == id);

            if (archivo == null)
            {
                return NotFound();
            }

            return View(archivo);
        }

        // GET: Archivos/Create
        public IActionResult Create(int claseId)
        {
            ViewBag.ClaseId = claseId;
            return View();
        }

        // POST: Archivos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int claseId, [Bind("Nombre,Tipo")] Archivo archivo, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    archivo.ClaseId = claseId;

                    if (file != null && file.Length > 0)
                    {
                        // Subir archivo a Google Drive
                        var clase = await _context.Clases
                            .Include(c => c.Subtema)
                            .ThenInclude(s => s.Unidad)
                            .ThenInclude(u => u.Materia)
                            .FirstOrDefaultAsync(c => c.ClaseId == claseId);

                        if (clase == null)
                        {
                            return NotFound();
                        }

                        // Crear una estructura de carpetas en Google Drive
                        string folderPath = $"{clase.Subtema.Unidad.Materia.Nombre}/{clase.Subtema.Unidad.Nombre}/{clase.Subtema.Nombre}/{clase.Nombre}";

                        // Subir el archivo
                        string fileId = await _driveService.UploadFileAsync(file, folderPath);
                        string fileUrl = await _driveService.GetFileLinkAsync(fileId);

                        archivo.DriveFileId = fileId;
                        archivo.DriveUrl = fileUrl;
                    }

                    archivo.FechaCreacion = DateTime.Now;
                    archivo.UltimaModificacion = DateTime.Now;

                    _context.Add(archivo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { claseId = archivo.ClaseId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al subir el archivo: {ex.Message}");
                }
            }
            ViewBag.ClaseId = claseId;
            return View(archivo);
        }

        // GET: Archivos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archivo = await _context.Archivos.FindAsync(id);
            if (archivo == null)
            {
                return NotFound();
            }
            return View(archivo);
        }

        // POST: Archivos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArchivoId,ClaseId,Nombre,Tipo,DriveFileId,DriveUrl")] Archivo archivo, IFormFile file)
        {
            if (id != archivo.ArchivoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingArchivo = await _context.Archivos.FindAsync(id);
                    if (existingArchivo == null)
                    {
                        return NotFound();
                    }

                    // Actualizar propiedades básicas
                    existingArchivo.Nombre = archivo.Nombre;
                    existingArchivo.Tipo = archivo.Tipo;
                    existingArchivo.UltimaModificacion = DateTime.Now;

                    // Si hay un nuevo archivo, reemplazar el existente
                    if (file != null && file.Length > 0)
                    {
                        // Eliminar el archivo anterior si existe
                        if (!string.IsNullOrEmpty(existingArchivo.DriveFileId))
                        {
                            await _driveService.DeleteFileAsync(existingArchivo.DriveFileId);
                        }

                        // Subir el nuevo archivo
                        var clase = await _context.Clases
                            .Include(c => c.Subtema)
                            .ThenInclude(s => s.Unidad)
                            .ThenInclude(u => u.Materia)
                            .FirstOrDefaultAsync(c => c.ClaseId == existingArchivo.ClaseId);

                        string folderPath = $"{clase.Subtema.Unidad.Materia.Nombre}/{clase.Subtema.Unidad.Nombre}/{clase.Subtema.Nombre}/{clase.Nombre}";

                        string fileId = await _driveService.UploadFileAsync(file, folderPath);
                        string fileUrl = await _driveService.GetFileLinkAsync(fileId);

                        existingArchivo.DriveFileId = fileId;
                        existingArchivo.DriveUrl = fileUrl;
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { claseId = existingArchivo.ClaseId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArchivoExists(archivo.ArchivoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(archivo);
        }

        // GET: Archivos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archivo = await _context.Archivos
                .Include(a => a.Clase)
                .FirstOrDefaultAsync(m => m.ArchivoId == id);
            if (archivo == null)
            {
                return NotFound();
            }

            return View(archivo);
        }

        // POST: Archivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var archivo = await _context.Archivos.FindAsync(id);

            // Eliminar el archivo de Google Drive
            if (!string.IsNullOrEmpty(archivo.DriveFileId))
            {
                await _driveService.DeleteFileAsync(archivo.DriveFileId);
            }

            _context.Archivos.Remove(archivo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { claseId = archivo.ClaseId });
        }

        private bool ArchivoExists(int id)
        {
            return _context.Archivos.Any(e => e.ArchivoId == id);
        }
    }
}