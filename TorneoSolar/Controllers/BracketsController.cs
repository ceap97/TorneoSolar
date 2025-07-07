using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoSolar.Models;

namespace TorneoSolar.Controllers
{
    public class BracketsController : Controller
    {
        private readonly TorneoSolarContext _context;

        public BracketsController(TorneoSolarContext context)
        {
            _context = context;
        }

        // GET: Brackets
        public async Task<IActionResult> Index()
        {
            var brackets = await _context.Brackets
                .Include(b => b.EquipoLocal)
                .Include(b => b.EquipoVisitante)
                .Include(b => b.Partido)
                .Include(b => b.Resultado)
                .Include(b => b.TablaPosicionesLocal)
                .Include(b => b.TablaPosicionesVisitante)
                .ToListAsync();

            // Diccionario de posiciones reales (por ranking)
            var tabla = await _context.TablaPosiciones
                .Include(tp => tp.Equipo)
                .OrderByDescending(x => (x.PG * 1.0 / (x.PJ == 0 ? 1 : x.PJ)))
                .ThenByDescending(x => x.PtsFavor - x.PtsContra)
                .ToListAsync();

            var posiciones = tabla
                .Select((tp, idx) => new { tp.EquipoId, Posicion = idx + 1 })
                .ToDictionary(x => x.EquipoId, x => x.Posicion);

            ViewBag.Posiciones = posiciones;

            return View(brackets);
        }

        // GET: Brackets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .Include(b => b.EquipoLocal)
                .Include(b => b.EquipoVisitante)
                .Include(b => b.Partido)
                .Include(b => b.Resultado)
                .Include(b => b.TablaPosicionesLocal)
                .Include(b => b.TablaPosicionesVisitante)
                .FirstOrDefaultAsync(m => m.BracketId == id);
            if (bracket == null)
            {
                return NotFound();
            }

            return View(bracket);
        }

        // GET: Brackets/Create
        public IActionResult Create()
        {
            var tabla = _context.TablaPosiciones
                .Include(tp => tp.Equipo)
                .OrderByDescending(x => (x.PG * 1.0 / (x.PJ == 0 ? 1 : x.PJ)))
                .ThenByDescending(x => x.PtsFavor - x.PtsContra)
                .ToList();

            var posiciones = tabla
                .Select((tp, idx) => new { tp.EquipoId, Posicion = idx + 1, Nombre = tp.Equipo.Nombre })
                .ToDictionary(x => x.EquipoId, x => new { x.Posicion, x.Nombre });

            var posicionesIds = tabla.ToDictionary(tp => tp.EquipoId, tp => tp.Id);

            ViewBag.PosicionesEquipos = posiciones;
            ViewBag.PosicionesIds = posicionesIds;

            var top8 = tabla.Take(8).ToList();
            var cruces = new List<(int localId, int visitanteId)>();
            int n = top8.Count;
            for (int i = 0; i < n / 2; i++)
            {
                cruces.Add((top8[i].EquipoId, top8[n - 1 - i].EquipoId));
            }

            var rondas = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Cuartos de final" },
                new SelectListItem { Value = "2", Text = "Semifinal" },
                new SelectListItem { Value = "3", Text = "Final" }
            };

            bool esPrimeraVez = !_context.Brackets.Any();
            ViewBag.EsPrimeraVez = esPrimeraVez;
            ViewBag.Rondas = esPrimeraVez
                ? rondas.Where(r => r.Value == "1").ToList()
                : rondas;

            var equiposLocales = cruces.Select(c => c.localId).ToList();
            var equiposVisitantes = cruces.Select(c => c.visitanteId).ToList();

            ViewData["EquiposCruce"] = cruces;
            ViewData["EquipoLocalId"] = new SelectList(_context.Equipos.Where(e => equiposLocales.Contains(e.EquipoId)), "EquipoId", "Nombre");
            ViewData["EquipoVisitanteId"] = new SelectList(_context.Equipos.Where(e => equiposVisitantes.Contains(e.EquipoId)), "EquipoId", "Nombre");
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId");
            ViewData["ResultadoId"] = new SelectList(_context.ResultadosPartidos, "ResultadoId", "ResultadoId");
            ViewData["TablaPosicionesLocalId"] = new SelectList(_context.TablaPosiciones, "Id", "Id");
            ViewData["TablaPosicionesVisitanteId"] = new SelectList(_context.TablaPosiciones, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("BracketId,EquipoLocalId,EquipoVisitanteId,Ronda,TablaPosicionesLocalId,TablaPosicionesVisitanteId")] Bracket bracket,
            DateTime FechaHora,
            string Ubicacion)
        {
            if (!_context.Brackets.Any())
            {
                bracket.Ronda = 1;
            }

            bracket.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                var partido = new Partido
                {
                    LocalEquipoId = bracket.EquipoLocalId,
                    VisitanteEquipoId = bracket.EquipoVisitanteId,
                    FechaHora = FechaHora,
                    Ubicacion = Ubicacion
                };
                _context.Partidos.Add(partido);
                await _context.SaveChangesAsync();

                var resultado = new ResultadosPartido
                {
                    PartidoId = partido.PartidoId,
                    PuntosLocal = 0,
                    PuntosVisitante = 0
                };
                _context.ResultadosPartidos.Add(resultado);
                await _context.SaveChangesAsync();

                bracket.PartidoId = partido.PartidoId;
                bracket.ResultadoId = resultado.ResultadoId;

                _context.Add(bracket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay error, recargar los combos y diccionarios para la vista
            var tabla = _context.TablaPosiciones
                .Include(tp => tp.Equipo)
                .OrderByDescending(x => (x.PG * 1.0 / (x.PJ == 0 ? 1 : x.PJ)))
                .ThenByDescending(x => x.PtsFavor - x.PtsContra)
                .ToList();

            var posiciones = tabla
                .Select((tp, idx) => new { tp.EquipoId, Posicion = idx + 1, Nombre = tp.Equipo.Nombre })
                .ToDictionary(x => x.EquipoId, x => new { x.Posicion, x.Nombre });

            var posicionesIds = tabla.ToDictionary(tp => tp.EquipoId, tp => tp.Id);

            ViewBag.PosicionesEquipos = posiciones;
            ViewBag.PosicionesIds = posicionesIds;

            var top8 = tabla.Take(8).ToList();
            var cruces = new List<(int localId, int visitanteId)>();
            int n = top8.Count;
            for (int i = 0; i < n / 2; i++)
            {
                cruces.Add((top8[i].EquipoId, top8[n - 1 - i].EquipoId));
            }

            var rondas = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Cuartos de final" },
                new SelectListItem { Value = "2", Text = "Semifinal" },
                new SelectListItem { Value = "3", Text = "Final" }
            };
            bool esPrimeraVez = !_context.Brackets.Any();
            ViewBag.EsPrimeraVez = esPrimeraVez;
            ViewBag.Rondas = esPrimeraVez
                ? rondas.Where(r => r.Value == "1").ToList()
                : rondas;

            var equiposLocales = cruces.Select(c => c.localId).ToList();
            var equiposVisitantes = cruces.Select(c => c.visitanteId).ToList();

            ViewData["EquiposCruce"] = cruces;
            ViewData["EquipoLocalId"] = new SelectList(_context.Equipos.Where(e => equiposLocales.Contains(e.EquipoId)), "EquipoId", "Nombre", bracket.EquipoLocalId);
            ViewData["EquipoVisitanteId"] = new SelectList(_context.Equipos.Where(e => equiposVisitantes.Contains(e.EquipoId)), "EquipoId", "Nombre", bracket.EquipoVisitanteId);
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", bracket.PartidoId);
            ViewData["ResultadoId"] = new SelectList(_context.ResultadosPartidos, "ResultadoId", "ResultadoId", bracket.ResultadoId);
            ViewData["TablaPosicionesLocalId"] = new SelectList(_context.TablaPosiciones, "Id", "Id", bracket.TablaPosicionesLocalId);
            ViewData["TablaPosicionesVisitanteId"] = new SelectList(_context.TablaPosiciones, "Id", "Id", bracket.TablaPosicionesVisitanteId);

            return View(bracket);
        }

        // GET: Brackets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .Include(b => b.EquipoLocal)
                .Include(b => b.EquipoVisitante)
                .FirstOrDefaultAsync(b => b.BracketId == id);

            if (bracket == null)
            {
                return NotFound();
            }

            var tabla = _context.TablaPosiciones
                .Include(tp => tp.Equipo)
                .OrderByDescending(x => (x.PG * 1.0 / (x.PJ == 0 ? 1 : x.PJ)))
                .ThenByDescending(x => x.PtsFavor - x.PtsContra)
                .ToList();

            var posiciones = tabla
                .Select((tp, idx) => new { tp.EquipoId, Posicion = idx + 1, Nombre = tp.Equipo.Nombre })
                .ToDictionary(x => x.EquipoId, x => new { x.Posicion, x.Nombre });

            var posicionesIds = tabla.ToDictionary(tp => tp.EquipoId, tp => tp.Id);

            ViewBag.PosicionesEquipos = posiciones;
            ViewBag.PosicionesIds = posicionesIds;

            ViewData["EquipoLocalId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", bracket.EquipoLocalId);
            ViewData["EquipoVisitanteId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", bracket.EquipoVisitanteId);

            var rondas = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Cuartos de final" },
                new SelectListItem { Value = "2", Text = "Semifinal" },
                new SelectListItem { Value = "3", Text = "Final" }
            };
            ViewBag.Rondas = rondas;

            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", bracket.PartidoId);
            ViewData["ResultadoId"] = new SelectList(_context.ResultadosPartidos, "ResultadoId", "ResultadoId", bracket.ResultadoId);
            ViewData["TablaPosicionesLocalId"] = new SelectList(_context.TablaPosiciones, "Id", "Id", bracket.TablaPosicionesLocalId);
            ViewData["TablaPosicionesVisitanteId"] = new SelectList(_context.TablaPosiciones, "Id", "Id", bracket.TablaPosicionesVisitanteId);

            return View(bracket);
        }

        // POST: Brackets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BracketId,PartidoId,ResultadoId,EquipoLocalId,EquipoVisitanteId,Ronda,TablaPosicionesLocalId,TablaPosicionesVisitanteId,FechaCreacion")] Bracket bracket)
        {
            if (id != bracket.BracketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bracket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BracketExists(bracket.BracketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var tabla = _context.TablaPosiciones
                .Include(tp => tp.Equipo)
                .OrderByDescending(x => (x.PG * 1.0 / (x.PJ == 0 ? 1 : x.PJ)))
                .ThenByDescending(x => x.PtsFavor - x.PtsContra)
                .ToList();

            var posiciones = tabla
                .Select((tp, idx) => new { tp.EquipoId, Posicion = idx + 1, Nombre = tp.Equipo.Nombre })
                .ToDictionary(x => x.EquipoId, x => new { x.Posicion, x.Nombre });

            var posicionesIds = tabla.ToDictionary(tp => tp.EquipoId, tp => tp.Id);

            ViewBag.PosicionesEquipos = posiciones;
            ViewBag.PosicionesIds = posicionesIds;

            ViewData["EquipoLocalId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", bracket.EquipoLocalId);
            ViewData["EquipoVisitanteId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", bracket.EquipoVisitanteId);

            var rondas = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Cuartos de final" },
                new SelectListItem { Value = "2", Text = "Semifinal" },
                new SelectListItem { Value = "3", Text = "Final" }
            };
            ViewBag.Rondas = rondas;

            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", bracket.PartidoId);
            ViewData["ResultadoId"] = new SelectList(_context.ResultadosPartidos, "ResultadoId", "ResultadoId", bracket.ResultadoId);
            ViewData["TablaPosicionesLocalId"] = new SelectList(_context.TablaPosiciones, "Id", "Id", bracket.TablaPosicionesLocalId);
            ViewData["TablaPosicionesVisitanteId"] = new SelectList(_context.TablaPosiciones, "Id", "Id", bracket.TablaPosicionesVisitanteId);

            return View(bracket);
        }

        private bool BracketExists(int id)
        {
            return _context.Brackets.Any(e => e.BracketId == id);
        }

        // GET: Brackets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bracket = await _context.Brackets
                .Include(b => b.EquipoLocal)
                .Include(b => b.EquipoVisitante)
                .Include(b => b.Partido)
                .Include(b => b.Resultado)
                .Include(b => b.TablaPosicionesLocal)
                .Include(b => b.TablaPosicionesVisitante)
                .FirstOrDefaultAsync(m => m.BracketId == id);
            if (bracket == null)
            {
                return NotFound();
            }

            return View(bracket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bracket = await _context.Brackets
                .Include(b => b.Resultado)
                .Include(b => b.Partido)
                .FirstOrDefaultAsync(b => b.BracketId == id);

            if (bracket != null)
            {
                // Eliminar el resultado si existe
                if (bracket.Resultado != null)
                {
                    _context.ResultadosPartidos.Remove(bracket.Resultado);
                }

                // Eliminar el partido si existe
                if (bracket.Partido != null)
                {
                    _context.Partidos.Remove(bracket.Partido);
                }

                // Eliminar el bracket
                _context.Brackets.Remove(bracket);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // --- Index1 (Bracket visual) se mantiene igual ---
        public async Task<IActionResult> Index1()
        {
            var tabla = await _context.TablaPosiciones
                .Include(tp => tp.Equipo)
                .OrderByDescending(x => (x.PG * 1.0 / (x.PJ == 0 ? 1 : x.PJ)))
                .ThenByDescending(x => x.PtsFavor - x.PtsContra)
                .ToListAsync();

            var posiciones = tabla
                .Select((tp, idx) => new { tp.EquipoId, Posicion = idx + 1 })
                .ToDictionary(x => x.EquipoId, x => x.Posicion);

            var brackets = await _context.Brackets
                .Include(b => b.EquipoLocal)
                .Include(b => b.EquipoVisitante)
                .Include(b => b.Resultado)
                .ToListAsync();

            var model = brackets.Select(b => new BracketMatch
            {
                Ronda = b.Ronda,
                EquipoLocal = b.EquipoLocal?.Nombre ?? "Por definir",
                EquipoVisitante = b.EquipoVisitante?.Nombre ?? "Por definir",
                PuntosLocal = b.Resultado?.PuntosLocal,
                PuntosVisitante = b.Resultado?.PuntosVisitante,
                PosicionLocal = b.EquipoLocalId != 0 && posiciones.ContainsKey(b.EquipoLocalId) ? posiciones[b.EquipoLocalId] : null,
                PosicionVisitante = b.EquipoVisitanteId != 0 && posiciones.ContainsKey(b.EquipoVisitanteId) ? posiciones[b.EquipoVisitanteId] : null
            }).ToList();

            return View(model);
        }
    }
}
