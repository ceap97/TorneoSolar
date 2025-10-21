using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoSolar.Models;

namespace TorneoSolar.Controllers
{
    public class JugadoresController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly ILogger<JugadoresController> _logger;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/jugadores");

        public JugadoresController(TorneoSolarContext context, ILogger<JugadoresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index(int? equipoId)
        {
            try
            {
                var jugadores = _context.Jugadores.AsQueryable();

                if (equipoId.HasValue)
                {
                    jugadores = jugadores.Where(j => j.EquipoId == equipoId.Value);
                }

                return View(jugadores.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Jugadores/Index equipoId={EquipoId}", equipoId);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        public async Task<IActionResult> Index1()
        {
            try
            {
                var torneoSolarContext = _context.Jugadores.Include(j => j.Equipo);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Jugadores/Index1");
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> Details1(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var jugadore = await _context.Jugadores
                    .Include(j => j.Equipo)
                    .FirstOrDefaultAsync(m => m.JugadorId == id);
                if (jugadore == null)
                {
                    return NotFound();
                }

                return View(jugadore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Jugadores/Details1 {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        // GET: Jugadores/Details/5
        [HttpGet]
        public async Task<IActionResult> GetJugadorDetails(int id)
        {
            try
            {
                var jugadore = await _context.Jugadores
                    .Include(j => j.Equipo)
                    .FirstOrDefaultAsync(m => m.JugadorId == id);

                if (jugadore == null)
                {
                    return NotFound();
                }

                return Json(new
                {
                    jugadore.Nombre,
                    jugadore.FechaNacimiento,
                    jugadore.Identificacion,
                    jugadore.Peso,
                    jugadore.Altura,
                    jugadore.Posicion,
                    Equipo = jugadore.Equipo?.Nombre
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles JSON del jugador {Id}", id);
                return Json(new { error = true, message = "Error interno" });
            }
        }

        [Authorize]

        // GET: Jugadores/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Jugadores/Create");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: Jugadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JugadorId,Nombre,FechaNacimiento,Identificacion,Peso,Altura,Posicion,EquipoId")] Jugadore jugadore, IFormFile foto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Si se proporciona una imagen, se guarda en el servidor
                    if (foto != null && foto.Length > 0)
                    {
                        var fileName = $"{jugadore.Nombre}.jpeg";  // Usar el nombre del jugador como nombre del archivo
                        var filePath = Path.Combine(_uploadPath, fileName);

                        // Crear el directorio si no existe
                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        // Guardar la imagen en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await foto.CopyToAsync(stream);
                        }

                        jugadore.Foto = $"/images/jugadores/{fileName}";  // Guardar la ruta relativa en la base de datos
                    }

                    _context.Add(jugadore);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index1));
                }
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", jugadore.EquipoId);
                return View(jugadore);
            }
            catch (Exception ex)
            {
                // Proporcionar un mensaje de error detallado
                _logger.LogError(ex, "Error al crear jugador {Nombre}", jugadore?.Nombre);
                ModelState.AddModelError(string.Empty, $"Error al crear el jugador: {ex.Message}");
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", jugadore.EquipoId);
                return View(jugadore);
            }
        }
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var jugadore = await _context.Jugadores.FindAsync(id);
                if (jugadore == null)
                {
                    return NotFound();
                }
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", jugadore.EquipoId);
                return View(jugadore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Jugadores/Edit {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: Jugadores1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JugadorId,Nombre,FechaNacimiento,Identificacion,Peso,Altura,Posicion,Foto,EquipoId")] Jugadore jugadore)
        {
            if (id != jugadore.JugadorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jugadore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JugadoreExists(jugadore.JugadorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Conflicto de concurrencia al editar jugador {Id}", jugadore.JugadorId);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index1));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", jugadore.EquipoId);
            return View(jugadore);
        }
        [Authorize]

        // GET: Jugadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var jugadore = await _context.Jugadores
                    .Include(j => j.Equipo)
                    .FirstOrDefaultAsync(m => m.JugadorId == id);
                if (jugadore == null)
                {
                    return NotFound();
                }

                return View(jugadore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Jugadores/Delete {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: Jugadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var jugadore = await _context.Jugadores.FindAsync(id);
                if (jugadore != null)
                {
                    _context.Jugadores.Remove(jugadore);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar jugador {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        private bool JugadoreExists(int id)
        {
            return _context.Jugadores.Any(e => e.JugadorId == id);
        }
    }
}

