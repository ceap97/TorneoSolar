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
    public class TablaPosicionesController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly ILogger<TablaPosicionesController> _logger;

        public TablaPosicionesController(TorneoSolarContext context, ILogger<TablaPosicionesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: TablaPosicioness
        public async Task<IActionResult> Index()
        {
            try
            {
                var torneoSolarContext = _context.TablaPosiciones.Include(t => t.Equipo);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TablaPosiciones/Index");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]
        public async Task<IActionResult> Index1()
        {
            try
            {
                var torneoSolarContext = _context.TablaPosiciones.Include(t => t.Equipo);
                return View(await torneoSolarContext.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TablaPosiciones/Index1");
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

                var TablaPosiciones = await _context.TablaPosiciones
                    .Include(t => t.Equipo)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (TablaPosiciones == null)
                {
                    return NotFound();
                }

                return View(TablaPosiciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en TablaPosiciones/Details {Id}", id);
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
                _logger.LogError(ex, "Error al cargar TablaPosiciones/Create");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: TablaPosicioness/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EquipoId,PJ,PG,PP,Puntos,PtsFavor,PtsContra")] TablaPosiciones TablaPosiciones)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(TablaPosiciones);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index1));
                }
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", TablaPosiciones.EquipoId);
                return View(TablaPosiciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear registro de TablaPosiciones");
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

                var TablaPosiciones = await _context.TablaPosiciones.FindAsync(id);
                if (TablaPosiciones == null)
                {
                    return NotFound();
                }
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosiciones.EquipoId);
                return View(TablaPosiciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar TablaPosiciones/Edit {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

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
                        _logger.LogError("Conflicto de concurrencia al editar TablaPosiciones {Id}", TablaPosiciones.Id);
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index1));
            }
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "EquipoId", TablaPosiciones.EquipoId);
            return View(TablaPosiciones);
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

                var TablaPosiciones = await _context.TablaPosiciones
                    .Include(t => t.Equipo)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (TablaPosiciones == null)
                {
                    return NotFound();
                }

                return View(TablaPosiciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar TablaPosiciones/Delete {Id}", id);
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
                var TablaPosiciones = await _context.TablaPosiciones.FindAsync(id);
                if (TablaPosiciones != null)
                {
                    _context.TablaPosiciones.Remove(TablaPosiciones);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar registro de TablaPosiciones {Id}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        private bool TablaPosicionesExists(int id)
        {
            return _context.TablaPosiciones.Any(e => e.Id == id);
        }
    }
}
