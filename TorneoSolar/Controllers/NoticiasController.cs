using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TorneoSolar.Models;

namespace TorneoSolar.Controllers
{
    [Authorize]

    public class NoticiasController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly ILogger<NoticiasController> _logger;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/noticias");

        public NoticiasController(TorneoSolarContext context, ILogger<NoticiasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Noticias
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Noticias.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Noticias/Index");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Index1()
        {
            try
            {
                var noticias = await _context.Noticias.ToListAsync();
                return View(noticias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Noticias/Index1");
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Noticias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var noticias = await _context.Noticias
                    .FirstOrDefaultAsync(m => m.NoticiasId == id);
                if (noticias == null)
                {
                    return NotFound();
                }

                return View(noticias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Noticias/Details {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Noticias/Create
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Noticias/Create");
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Noticias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoticiasId,Titulo,Comentario,Fecha")] Noticias noticias, IFormFile imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Si se proporciona una imagen, se guarda en el servidor
                    if (imagen != null && imagen.Length > 0)
                    {
                        var fileName = $"{noticias.Titulo}.jpeg";  // Usar el título de la noticia como nombre del archivo
                        var filePath = Path.Combine(_uploadPath, fileName);

                        // Crear el directorio si no existe
                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        // Guardar la imagen en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imagen.CopyToAsync(stream);
                        }

                        noticias.Imagen = $"/images/noticias/{fileName}";  // Guardar la ruta relativa en la base de datos
                    }

                    _context.Add(noticias);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(noticias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear noticia {Titulo}", noticias?.Titulo);
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: Noticias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var noticias = await _context.Noticias.FindAsync(id);
                if (noticias == null)
                {
                    return NotFound();
                }
                return View(noticias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Noticias/Edit {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Noticias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoticiasId,Titulo,Comentario,Fecha,Imagen")] Noticias noticias)
        {
            if (id != noticias.NoticiasId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noticias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticiasExists(noticias.NoticiasId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Conflicto de concurrencia al editar noticia {Id}", noticias.NoticiasId);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(noticias);
        }

        // GET: Noticias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var noticias = await _context.Noticias
                    .FirstOrDefaultAsync(m => m.NoticiasId == id);
                if (noticias == null)
                {
                    return NotFound();
                }

                return View(noticias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Noticias/Delete {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var noticias = await _context.Noticias.FindAsync(id);
                if (noticias != null)
                {
                    _context.Noticias.Remove(noticias);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar noticia {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        private bool NoticiasExists(int id)
        {
            return _context.Noticias.Any(e => e.NoticiasId == id);
        }
    }
}
