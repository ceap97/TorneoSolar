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
    public class Jugadores1Controller : Controller
    {
        private readonly TorneoSolarContext _context;

        public Jugadores1Controller(TorneoSolarContext context)
        {
            _context = context;
        }

        // GET: Jugadores1
        public async Task<IActionResult> Index()
        {
            var torneoSolarContext = _context.Jugadores.Include(j => j.Equipo);
            return View(await torneoSolarContext.ToListAsync());
        }

        // GET: Jugadores1/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Jugadores1/Create
        public IActionResult Create()
        {
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId");
            return View();
        }

        // POST: Jugadores1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JugadorId,Nombre,FechaNacimiento,Identificacion,Peso,Altura,Posicion,Foto,EquipoId")] Jugadore jugadore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jugadore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", jugadore.EquipoId);
            return View(jugadore);
        }

        // GET: Jugadores1/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", jugadore.EquipoId);
            return View(jugadore);
        }

        // GET: Jugadores1/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Jugadores1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jugadore = await _context.Jugadores.FindAsync(id);
            if (jugadore != null)
            {
                _context.Jugadores.Remove(jugadore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JugadoreExists(int id)
        {
            return _context.Jugadores.Any(e => e.JugadorId == id);
        }
    }
}
