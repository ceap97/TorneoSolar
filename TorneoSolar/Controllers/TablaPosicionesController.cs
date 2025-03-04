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
    public class TablaPosicionesController : Controller
    {
        private readonly TorneoSolarContext _context;

        public TablaPosicionesController(TorneoSolarContext context)
        {
            _context = context;
        }

        // GET: TablaPosicioness
        public async Task<IActionResult> Index()
        {
            var torneoSolarContext = _context.TablaPosiciones.Include(t => t.Equipo);
            return View(await torneoSolarContext.ToListAsync());
        }
        public async Task<IActionResult> Index1()
        {
            var torneoSolarContext = _context.TablaPosiciones.Include(t => t.Equipo);
            return View(await torneoSolarContext.ToListAsync());
        }

        // GET: TablaPosicioness/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var TablaPosiciones = await _context.TablaPosiciones
                .Include(t => t.Equipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (TablaPosiciones == null)
            {
                return NotFound();
            }

            return View(TablaPosiciones);
        }

        // GET: TablaPosicioness/Create
        public IActionResult Create()
        {
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId");
            return View();
        }

        // POST: TablaPosicioness/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EquipoId,PJ,PG,PP,Puntos,PtsFavor,PtsContra")] TablaPosiciones TablaPosiciones)
        {
            if (ModelState.IsValid)
            {
                _context.Add(TablaPosiciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosiciones.EquipoId);
            return View(TablaPosiciones);
        }


        // GET: TablaPosicioness/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var TablaPosiciones = await _context.TablaPosiciones.FindAsync(id);
            if (TablaPosiciones == null)
            {
                return NotFound();
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosiciones.EquipoId);
            return View(TablaPosiciones);
        }

        // POST: TablaPosicioness/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EquipoId,PJ,PG,PP,Puntos,PtsFavor,PtsContra")] TablaPosiciones TablaPosiciones)
        {
            if (id != TablaPosiciones.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(TablaPosiciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TablaPosicionesExists(TablaPosiciones.Id))
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
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosiciones.EquipoId);
            return View(TablaPosiciones);
        }


        // GET: TablaPosicioness/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var TablaPosiciones = await _context.TablaPosiciones
                .Include(t => t.Equipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (TablaPosiciones == null)
            {
                return NotFound();
            }

            return View(TablaPosiciones);
        }

        // POST: TablaPosicioness/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var TablaPosiciones = await _context.TablaPosiciones.FindAsync(id);
            if (TablaPosiciones != null)
            {
                _context.TablaPosiciones.Remove(TablaPosiciones);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TablaPosicionesExists(int id)
        {
            return _context.TablaPosiciones.Any(e => e.Id == id);
        }
    }
}
