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
    public class ResultadosPartidosController : Controller
    {
        private readonly TorneoSolarContext _context;

        public ResultadosPartidosController(TorneoSolarContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<JsonResult> GetDetallesPartido(int partidoId)
        {
            var partido = await _context.Partidos
                .Include(p => p.LocalEquipo)
                .Include(p => p.VisitanteEquipo)
                .FirstOrDefaultAsync(p => p.PartidoId == partidoId);

            if (partido == null)
            {
                return Json(new { success = false, message = "Partido no encontrado" });
            }

            var detallesPartido = new
            {
                partido.PartidoId,
                LocalEquipo = partido.LocalEquipo.Nombre,
                VisitanteEquipo = partido.VisitanteEquipo.Nombre,
                FechaHora = partido.FechaHora
            };

            return Json(new { success = true, detallesPartido });
        }

        private async Task RevertirTablaPosiciones(ResultadosPartido resultadosPartido)
        {
            var partido = await _context.Partidos
                .Include(p => p.LocalEquipo)
                .Include(p => p.VisitanteEquipo)
                .FirstOrDefaultAsync(p => p.PartidoId == resultadosPartido.PartidoId);

            if (partido == null) return;

            var equipoLocal = await _context.TablaPosiciones.FirstOrDefaultAsync(tp => tp.EquipoId == partido.LocalEquipoId);
            var equipoVisitante = await _context.TablaPosiciones.FirstOrDefaultAsync(tp => tp.EquipoId == partido.VisitanteEquipoId);

            if (equipoLocal == null || equipoVisitante == null) return;

            equipoLocal.PJ--;
            equipoVisitante.PJ--;

            equipoLocal.PtsFavor -= resultadosPartido.PuntosLocal;
            equipoLocal.PtsContra -= resultadosPartido.PuntosVisitante;

            equipoVisitante.PtsFavor -= resultadosPartido.PuntosVisitante;
            equipoVisitante.PtsContra -= resultadosPartido.PuntosLocal;

            if (resultadosPartido.PuntosLocal > resultadosPartido.PuntosVisitante)
            {
                equipoLocal.PG--;
                equipoLocal.Puntos -= 2;
                equipoVisitante.PP--;
            }
            else if (resultadosPartido.PuntosLocal < resultadosPartido.PuntosVisitante)
            {
                equipoVisitante.PG--;
                equipoVisitante.Puntos -= 2;
                equipoLocal.PP--;
            }

            equipoLocal.Diferencia = equipoLocal.PtsFavor - equipoLocal.PtsContra;
            equipoVisitante.Diferencia = equipoVisitante.PtsFavor - equipoVisitante.PtsContra;

            _context.Update(equipoLocal);
            _context.Update(equipoVisitante);
            await _context.SaveChangesAsync();
        }
        private async Task ActualizarTablaPosiciones(ResultadosPartido resultadosPartido)
        {
            var partido = await _context.Partidos
                .Include(p => p.LocalEquipo)
                .Include(p => p.VisitanteEquipo)
                .FirstOrDefaultAsync(p => p.PartidoId == resultadosPartido.PartidoId);

            if (partido == null) return;

            var equipoLocal = await _context.TablaPosiciones.FirstOrDefaultAsync(tp => tp.EquipoId == partido.LocalEquipoId);
            var equipoVisitante = await _context.TablaPosiciones.FirstOrDefaultAsync(tp => tp.EquipoId == partido.VisitanteEquipoId);

            if (equipoLocal == null || equipoVisitante == null) return;

            equipoLocal.PJ++;
            equipoVisitante.PJ++;

            equipoLocal.PtsFavor += resultadosPartido.PuntosLocal;
            equipoLocal.PtsContra += resultadosPartido.PuntosVisitante;

            equipoVisitante.PtsFavor += resultadosPartido.PuntosVisitante;
            equipoVisitante.PtsContra += resultadosPartido.PuntosLocal;

            if (resultadosPartido.PuntosLocal > resultadosPartido.PuntosVisitante)
            {
                equipoLocal.PG++;
                equipoLocal.Puntos += 2;
                equipoVisitante.PP++;
            }
            else if (resultadosPartido.PuntosLocal < resultadosPartido.PuntosVisitante)
            {
                equipoVisitante.PG++;
                equipoVisitante.Puntos += 2;
                equipoLocal.PP++;
            }

            equipoLocal.Diferencia = equipoLocal.PtsFavor - equipoLocal.PtsContra;
            equipoVisitante.Diferencia = equipoVisitante.PtsFavor - equipoVisitante.PtsContra;

            _context.Update(equipoLocal);
            _context.Update(equipoVisitante);
            await _context.SaveChangesAsync();
        }

        // GET: ResultadosPartidos
        public async Task<IActionResult> Index()
        {
            var torneoSolarContext = _context.ResultadosPartidos.Include(r => r.Partido);
            return View(await torneoSolarContext.ToListAsync());
        }
        public async Task<IActionResult> Index1()
        {
            var torneoSolarContext = _context.ResultadosPartidos.Include(r => r.Partido);
            return View(await torneoSolarContext.ToListAsync());
        }
        [Authorize]

        // GET: ResultadosPartidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadosPartido = await _context.ResultadosPartidos
                .Include(r => r.Partido)
                .FirstOrDefaultAsync(m => m.ResultadoId == id);
            if (resultadosPartido == null)
            {
                return NotFound();
            }

            return View(resultadosPartido);
        }

        // GET: ResultadosPartidos/Create
        [Authorize]

        public IActionResult Create()
        {
            ViewData["PartidoId"] = new SelectList(_context.Partidos
                .Include(p => p.LocalEquipo)
                .Include(p => p.VisitanteEquipo)
                .Select(p => new
                {
                    p.PartidoId,
                    NombrePartido = $"{p.LocalEquipo.Nombre} vs {p.VisitanteEquipo.Nombre} - {p.FechaHora}"
                }), "PartidoId", "NombrePartido");
            return View();
        }

        [Authorize]

        // POST: ResultadosPartidos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResultadoId,PartidoId,PuntosLocal,PuntosVisitante")] ResultadosPartido resultadosPartido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultadosPartido);
                await _context.SaveChangesAsync();
                await ActualizarTablaPosiciones(resultadosPartido);
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", resultadosPartido.PartidoId);
            return View(resultadosPartido);
        }


        // GET: ResultadosPartidos/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadosPartido = await _context.ResultadosPartidos.FindAsync(id);
            if (resultadosPartido == null)
            {
                return NotFound();
            }
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", resultadosPartido.PartidoId);
            return View(resultadosPartido);
        }
        [Authorize]

        // POST: ResultadosPartidos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResultadoId,PartidoId,PuntosLocal,PuntosVisitante")] ResultadosPartido resultadosPartido)
        {
            if (id != resultadosPartido.ResultadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var resultadoAnterior = await _context.ResultadosPartidos.AsNoTracking().FirstOrDefaultAsync(r => r.ResultadoId == id);
                    if (resultadoAnterior != null)
                    {
                        await RevertirTablaPosiciones(resultadoAnterior);
                    }

                    _context.Update(resultadosPartido);
                    await _context.SaveChangesAsync();
                    await ActualizarTablaPosiciones(resultadosPartido);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultadosPartidoExists(resultadosPartido.ResultadoId))
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
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", resultadosPartido.PartidoId);
            return View(resultadosPartido);
        }

        // GET: ResultadosPartidos/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadosPartido = await _context.ResultadosPartidos
                .Include(r => r.Partido)
                .FirstOrDefaultAsync(m => m.ResultadoId == id);
            if (resultadosPartido == null)
            {
                return NotFound();
            }

            return View(resultadosPartido);
        }
        [Authorize]

        // POST: ResultadosPartidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultadosPartido = await _context.ResultadosPartidos.FindAsync(id);
            if (resultadosPartido != null)
            {
                await RevertirTablaPosiciones(resultadosPartido);
                _context.ResultadosPartidos.Remove(resultadosPartido);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ResultadosPartidoExists(int id)
        {
            return _context.ResultadosPartidos.Any(e => e.ResultadoId == id);
        }
    }
}
