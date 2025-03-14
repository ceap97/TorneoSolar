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
    public class TablaPosicionesFemController : Controller
    {
        private readonly TorneoSolarContext _context;

        public TablaPosicionesFemController(TorneoSolarContext context)
        {
            _context = context;
        }

        // GET: TablaPosicioness
        public async Task<IActionResult> Index()
        {
            var torneoSolarContext = _context.TablaPosicionesFem.Include(t => t.Equipo);
            return View(await torneoSolarContext.ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> Index1()
        {
            var torneoSolarContext = _context.TablaPosicionesFem.Include(t => t.Equipo);
            return View(await torneoSolarContext.ToListAsync());
        }
        [Authorize]

        // GET: TablaPosicioness/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var TablaPosicionesFem = await _context.TablaPosicionesFem
                .Include(t => t.Equipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (TablaPosicionesFem == null)
            {
                return NotFound();
            }

            return View(TablaPosicionesFem);
        }

        [Authorize]


        // GET: TablaPosicioness/Create
        public IActionResult Create()
        {
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
            return View();
        }

        [Authorize]

        // POST: TablaPosicioness/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EquipoId,PJ,PG,PP,Puntos,PtsFavor,PtsContra")] TablaPosicionesFem TablaPosicionesFem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(TablaPosicionesFem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", TablaPosicionesFem.EquipoId);
            return View(TablaPosicionesFem);
        }


        [Authorize]

        // GET: TablaPosicioness/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var TablaPosicionesFem = await _context.TablaPosicionesFem.FindAsync(id);
            if (TablaPosicionesFem == null)
            {
                return NotFound();
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosicionesFem.EquipoId);
            return View(TablaPosicionesFem);
        }
        [Authorize]

        // POST: TablaPosicioness/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EquipoId,PJ,PG,PP,Puntos,PtsFavor,PtsContra")] TablaPosicionesFem TablaPosicionesFem)
        {
            if (id != TablaPosicionesFem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(TablaPosicionesFem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TablaPosicionesExists(TablaPosicionesFem.Id))
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
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosicionesFem.EquipoId);
            return View(TablaPosicionesFem);
        }

        [Authorize]

        // GET: TablaPosicioness/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var TablaPosicionesFem = await _context.TablaPosicionesFem
                .Include(t => t.Equipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (TablaPosicionesFem == null)
            {
                return NotFound();
            }

            return View(TablaPosicionesFem);
        }
        [Authorize]

        // POST: TablaPosicioness/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var TablaPosicionesFem = await _context.TablaPosicionesFem.FindAsync(id);
            if (TablaPosicionesFem != null)
            {
                _context.TablaPosicionesFem.Remove(TablaPosicionesFem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TablaPosicionesExists(int id)
        {
            return _context.TablaPosicionesFem.Any(e => e.Id == id);
        }
    }
}
