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
        private readonly ILogger<TablaPosicionesFemController> _logger;

        public TablaPosicionesFemController(TorneoSolarContext context, ILogger<TablaPosicionesFemController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: TablaPosicioness
        public async Task<IActionResult> Index()
        {
            try
            {
                var torneoSolarContext = _context.TablaPosicionesFem.Include(t => t.Equipo);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TablaPosicionesFem/Index");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]
        public async Task<IActionResult> Index1()
        {
            try
            {
                var torneoSolarContext = _context.TablaPosicionesFem.Include(t => t.Equipo);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TablaPosicionesFem/Index1");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // GET: TablaPosicioness/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TablaPosicionesFem/Details {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]


        // GET: TablaPosicioness/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar TablaPosicionesFem/Create");
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]

        // POST: TablaPosicioness/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EquipoId,PJ,PG,PP,Puntos,PtsFavor,PtsContra")] TablaPosicionesFem TablaPosicionesFem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(TablaPosicionesFem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index1));
                }
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", TablaPosicionesFem.EquipoId);
                return View(TablaPosicionesFem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear registro TablaPosicionesFem");
                return RedirectToAction("Error", "Home");
            }
        }


        [Authorize]

        // GET: TablaPosicioness/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar TablaPosicionesFem/Edit {Id}", id);
                return RedirectToAction("Error", "Home");
            }
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
                        _logger.LogError("Conflicto de concurrencia al editar TablaPosicionesFem {Id}", TablaPosicionesFem.Id);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index1));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosicionesFem.EquipoId);
            return View(TablaPosicionesFem);
        }

        [Authorize]

        // GET: TablaPosicioness/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar TablaPosicionesFem/Delete {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: TablaPosicioness/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var TablaPosicionesFem = await _context.TablaPosicionesFem.FindAsync(id);
                if (TablaPosicionesFem != null)
                {
                    _context.TablaPosicionesFem.Remove(TablaPosicionesFem);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar TablaPosicionesFem {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        private bool TablaPosicionesExists(int id)
        {
            return _context.TablaPosicionesFem.Any(e => e.Id == id);
        }
    }
}
