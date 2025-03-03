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
    public class EstadisticasJugadoresController : Controller
    {
        private readonly TorneoSolarContext _context;

        public EstadisticasJugadoresController(TorneoSolarContext context)
        {
            _context = context;
        }

        // GET: EstadisticasJugadores
        public async Task<IActionResult> Index()
        {
            var torneoSolarContext = _context.EstadisticasJugadores.Include(e => e.Jugador).Include(e => e.Partido);
            return View(await torneoSolarContext.ToListAsync());
        }
        public async Task<IActionResult> Index1()
        {
            var torneoSolarContext = _context.EstadisticasJugadores.Include(e => e.Jugador).Include(e => e.Partido);
            return View(await torneoSolarContext.ToListAsync());
        }

        // GET: EstadisticasJugadores/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: EstadisticasJugadores/Create
        public IActionResult Create()
        {
            ViewData["JugadorId"] = new SelectList(_context.Jugadores, "JugadorId", "JugadorId");
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId");
            return View();
        }

        // POST: EstadisticasJugadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstadisticaId,JugadorId,PartidoId,Puntos,Rebotes,Asistencias,Bloqueos,Robos,MinutosJugados")] EstadisticasJugadore estadisticasJugadore)
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

        // GET: EstadisticasJugadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: EstadisticasJugadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadisticasJugadore = await _context.EstadisticasJugadores.FindAsync(id);
            if (estadisticasJugadore != null)
            {
                _context.EstadisticasJugadores.Remove(estadisticasJugadore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadisticasJugadoreExists(int id)
        {
            return _context.EstadisticasJugadores.Any(e => e.EstadisticaId == id);
        }
    }
}
