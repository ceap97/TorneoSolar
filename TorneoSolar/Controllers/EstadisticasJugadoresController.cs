using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoSolar.Models;

namespace TorneoSolar.Controllers
{
    [Authorize]

    public class EstadisticasJugadoresController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly ILogger<EstadisticasJugadoresController> _logger;

        public EstadisticasJugadoresController(TorneoSolarContext context, ILogger<EstadisticasJugadoresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<JsonResult> GetJugadoresPorPartido(int partidoId)
        {
            try
            {
                var partido = await _context.Partidos
                    .Include(p => p.LocalEquipo)
                    .Include(p => p.VisitanteEquipo)
                    .FirstOrDefaultAsync(p => p.PartidoId == partidoId);

                if (partido == null)
                {
                    return Json(new { success = false, message = "Partido no encontrado" });
                }

                var jugadores = await _context.Jugadores
                    .Where(j => j.EquipoId == partido.LocalEquipoId || j.EquipoId == partido.VisitanteEquipoId)
                    .Select(j => new { j.JugadorId, j.Nombre })
                    .ToListAsync();

                return Json(new { success = true, jugadores });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener jugadores por partido {PartidoId}", partidoId);
                return Json(new { success = false, message = "Error interno" });
            }
        }

        // GET: EstadisticasJugadores
        public async Task<IActionResult> Index()
        {
            try
            {
                var torneoSolarContext = _context.EstadisticasJugadores.Include(e => e.Jugador).Include(e => e.Partido);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en EstadisticasJugadores/Index");
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> Index1()
        {
            try
            {
                var torneoSolarContext = _context.EstadisticasJugadores.Include(e => e.Jugador).Include(e => e.Partido);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en EstadisticasJugadores/Index1");
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: EstadisticasJugadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var estadisticasJugadore = await _context.EstadisticasJugadores
                    .Include(e => e.Jugador)
                    .Include(e => e.Partido)
                    .FirstOrDefaultAsync(m => m.EstadisticaId == id);
                if (estadisticasJugadore == null)
                {
                    return NotFound();
                }

                return View(estadisticasJugadore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en EstadisticasJugadores/Details {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: EstadisticasJugadores/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["PartidoId"] = new SelectList(_context.Partidos
                    .Include(p => p.LocalEquipo)
                    .Include(p => p.VisitanteEquipo)
                    .Select(p => new
                    {
                        p.PartidoId,
                        NombrePartido = $"{p.LocalEquipo.Nombre} vs {p.VisitanteEquipo.Nombre} - {p.FechaHora}"
                    }), "PartidoId", "NombrePartido");

                ViewData["JugadorId"] = new SelectList(Enumerable.Empty<SelectListItem>(), "JugadorId", "Nombre");

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar EstadisticasJugadores/Create");
                return RedirectToAction("Error", "Home");
            }
        }


        // POST: EstadisticasJugadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstadisticaId,JugadorId,PartidoId,Puntos,Rebotes,Asistencias,Bloqueos,Robos,MinutosJugados")] EstadisticasJugadore estadisticasJugadore)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(estadisticasJugadore);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["JugadorId"] = new SelectList(_context.Jugadores, "JugadorId", "JugadorId", estadisticasJugadore.JugadorId);
                ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", estadisticasJugadore.PartidoId);
                return View(estadisticasJugadore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear estadística de jugador {JugadorId}", estadisticasJugadore?.JugadorId);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: EstadisticasJugadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var estadisticasJugadore = await _context.EstadisticasJugadores.FindAsync(id);
                if (estadisticasJugadore == null)
                {
                    return NotFound();
                }
                ViewData["JugadorId"] = new SelectList(_context.Jugadores, "JugadorId", "JugadorId", estadisticasJugadore.JugadorId);
                ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", estadisticasJugadore.PartidoId);
                return View(estadisticasJugadore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar EstadisticasJugadores/Edit {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: EstadisticasJugadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstadisticaId,JugadorId,PartidoId,Puntos,Rebotes,Asistencias,Bloqueos,Robos,MinutosJugados")] EstadisticasJugadore estadisticasJugadore)
        {
            if (id != estadisticasJugadore.EstadisticaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadisticasJugadore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadisticasJugadoreExists(estadisticasJugadore.EstadisticaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Conflicto de concurrencia al editar estadística {Id}", estadisticasJugadore.EstadisticaId);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["JugadorId"] = new SelectList(_context.Jugadores, "JugadorId", "JugadorId", estadisticasJugadore.JugadorId);
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", estadisticasJugadore.PartidoId);
            return View(estadisticasJugadore);
        }

        // GET: EstadisticasJugadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var estadisticasJugadore = await _context.EstadisticasJugadores
                    .Include(e => e.Jugador)
                    .Include(e => e.Partido)
                    .FirstOrDefaultAsync(m => m.EstadisticaId == id);
                if (estadisticasJugadore == null)
                {
                    return NotFound();
                }

                return View(estadisticasJugadore);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar EstadisticasJugadores/Delete {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: EstadisticasJugadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var estadisticasJugadore = await _context.EstadisticasJugadores.FindAsync(id);
                if (estadisticasJugadore != null)
                {
                    _context.EstadisticasJugadores.Remove(estadisticasJugadore);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar EstadisticasJugadores {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        private bool EstadisticasJugadoreExists(int id)
        {
            return _context.EstadisticasJugadores.Any(e => e.EstadisticaId == id);
        }
    }
}
