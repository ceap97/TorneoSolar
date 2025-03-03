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
    public class ResultadosPartidosController : Controller
    {
        private readonly TorneoSolarContext _context;

        public ResultadosPartidosController(TorneoSolarContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId");
            return View();
        }

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
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartidoId"] = new SelectList(_context.Partidos, "PartidoId", "PartidoId", resultadosPartido.PartidoId);
            return View(resultadosPartido);
        }

        // GET: ResultadosPartidos/Edit/5
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

        // POST: ResultadosPartidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(resultadosPartido);
                    await _context.SaveChangesAsync();
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

        // POST: ResultadosPartidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultadosPartido = await _context.ResultadosPartidos.FindAsync(id);
            if (resultadosPartido != null)
            {
                _context.ResultadosPartidos.Remove(resultadosPartido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultadosPartidoExists(int id)
        {
            return _context.ResultadosPartidos.Any(e => e.ResultadoId == id);
        }
    }
}
