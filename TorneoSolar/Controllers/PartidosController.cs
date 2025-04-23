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

        public PartidosController(TorneoSolarContext context)
        {
            _context = context;
        }
        [HttpGet]
        public JsonResult GetVisitantesDisponibles(int localId)
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


        public async Task<IActionResult> UltimosResultados()
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

        public async Task<IActionResult> Index()
        {
            var partidosConResultados = _context.Partidos
                .Include(p => p.LocalEquipo)
                .Include(p => p.VisitanteEquipo)
                .Include(p => p.ResultadosPartido)
                .OrderBy(p => p.FechaHora); // Ordenar por FechaHora

            return View(await partidosConResultados.ToListAsync());
        }

        [Authorize]

        public async Task<IActionResult> Index1()
        {
            var torneoSolarContext = _context.Partidos.Include(p => p.LocalEquipo).Include(p => p.VisitanteEquipo);
            return View(await torneoSolarContext.ToListAsync());
        }
        [Authorize]

        // GET: Partidos/Details/5
        public async Task<IActionResult> Details(int? id)
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
        [Authorize]

        // GET: Partidos/Create
        public IActionResult Create()
        {
            ViewData["LocalEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
            ViewData["VisitanteEquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
            return View();
        }
        [Authorize]

        // POST: Partidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PartidoId,FechaHora,LocalEquipoId,VisitanteEquipoId,Ubicacion")] Partido partido)
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
        [Authorize]

        // GET: Partidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        [Authorize]

        // POST: Partidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partido = await _context.Partidos.FindAsync(id);
            if (partido != null)
            {
                _context.Partidos.Remove(partido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index1));
        }

        private bool PartidoExists(int id)
        {
            return _context.Partidos.Any(e => e.PartidoId == id);
        }
    }
}
