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
    public class PartidosController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly ILogger<PartidosController> _logger;

        public PartidosController(TorneoSolarContext context, ILogger<PartidosController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public JsonResult GetVisitantesDisponibles(int localId)
        {
            try
            {
                var equipoLocal = _context.Equipos.FirstOrDefault(e => e.EquipoId == localId);

                if (equipoLocal == null)
                {
                    return Json(new List<object>());
                }

                bool esFemenino = equipoLocal.Nombre.Contains("(Femenino)");

                if (esFemenino)
                {
                    // Si el equipo local es femenino, mostrar solo otros equipos femeninos, sin importar si ya jugaron
                    var visitantesFemeninos = _context.Equipos
                        .Where(e => e.EquipoId != localId && e.Nombre.Contains("(Femenino)"))
                        .Select(e => new { e.EquipoId, e.Nombre })
                        .ToList();

                    return Json(visitantesFemeninos);
                }

                // Si el equipo local no es femenino, excluir equipos femeninos y los ya jugados
                var partidosJugados = _context.Partidos
                    .Where(p => p.LocalEquipoId == localId || p.VisitanteEquipoId == localId)
                    .Select(p => new { p.LocalEquipoId, p.VisitanteEquipoId })
                    .ToList();

                var equiposYaJugados = partidosJugados
                    .SelectMany(p => new[] { p.LocalEquipoId, p.VisitanteEquipoId })
                    .Where(id => id != localId)
                    .Distinct()
                    .ToList();

                var visitantesDisponibles = _context.Equipos
                    .Where(e =>
                        e.EquipoId != localId &&
                        !e.Nombre.Contains("(Femenino)") && // excluir equipos femeninos
                        !equiposYaJugados.Contains(e.EquipoId)) // excluir equipos ya enfrentados
                    .Select(e => new { e.EquipoId, e.Nombre })
                    .ToList();

                return Json(visitantesDisponibles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetVisitantesDisponibles localId={LocalId}", localId);
                return Json(Array.Empty<object>());
            }
        }


        public async Task<IActionResult> UltimosResultados()
        {
            try
            {
                var ultimosResultados = await _context.Partidos
                    .Include(p => p.LocalEquipo)
                    .Include(p => p.VisitanteEquipo)
                    .Include(p => p.ResultadosPartido)
                    .OrderByDescending(p => p.FechaHora)
                    .Take(5) // Obtener los últimos 5 resultados
                    .ToListAsync();

                return PartialView("_UltimosResultados", ultimosResultados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener últimos resultados");
                return PartialView("_UltimosResultados", new List<Partido>());
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var partidosConResultados = _context.Partidos
                    .Include(p => p.LocalEquipo)
                    .Include(p => p.VisitanteEquipo)
                    .Include(p => p.ResultadosPartido)
                    .OrderBy(p => p.FechaHora); // Ordenar por FechaHora

                return View(await partidosConResultados.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Partidos/Index");
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]

        public async Task<IActionResult> Index1()
        {
            try
            {
                var torneoSolarContext = _context.Partidos.Include(p => p.LocalEquipo).Include(p => p.VisitanteEquipo);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Partidos/Index1");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // GET: Partidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var partido = await _context.Partidos
                    .Include(p => p.LocalEquipo)
                    .Include(p => p.VisitanteEquipo)
                    .FirstOrDefaultAsync(m => m.PartidoId == id);
                if (partido == null)
                {
                    return NotFound();
                }

                return View(partido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Partidos/Details {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // GET: Partidos/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["LocalEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
                ViewData["VisitanteEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Partidos/Create");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: Partidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartidoId,FechaHora,LocalEquipoId,VisitanteEquipoId,Ubicacion")] Partido partido)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(partido);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index1));
                }
                ViewData["LocalEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", partido.LocalEquipoId);
                ViewData["VisitanteEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", partido.VisitanteEquipoId);
                return View(partido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear Partido");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // GET: Partidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var partido = await _context.Partidos.FindAsync(id);
                if (partido == null)
                {
                    return NotFound();
                }
                ViewData["LocalEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", partido.LocalEquipoId);
                ViewData["VisitanteEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", partido.VisitanteEquipoId);
                return View(partido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Partidos/Edit {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: Partidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PartidoId,FechaHora,LocalEquipoId,VisitanteEquipoId,Ubicacion")] Partido partido)
        {
            if (id != partido.PartidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartidoExists(partido.PartidoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Conflicto de concurrencia al editar partido {Id}", partido.PartidoId);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index1));
            }
            ViewData["LocalEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", partido.LocalEquipoId);
            ViewData["VisitanteEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", partido.VisitanteEquipoId);
            return View(partido);
        }

        // GET: Partidos/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var partido = await _context.Partidos
                    .Include(p => p.LocalEquipo)
                    .Include(p => p.VisitanteEquipo)
                    .FirstOrDefaultAsync(m => m.PartidoId == id);
                if (partido == null)
                {
                    return NotFound();
                }

                return View(partido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Partidos/Delete {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: Partidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var partido = await _context.Partidos.FindAsync(id);
                if (partido != null)
                {
                    _context.Partidos.Remove(partido);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar Partido {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        private bool PartidoExists(int id)
        {
            return _context.Partidos.Any(e => e.PartidoId == id);
        }
    }
}
