using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TorneoSolar.Models;

namespace TorneoSolar.Controllers
{
    public class JugadoresController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/jugadores");

        public JugadoresController(TorneoSolarContext context)
        {
            _context = context;
        }

        // GET: Jugadores
        public async Task<IActionResult> Index()
        {
            var torneoSolarContext = _context.Jugadores.Include(j => j.Equipo);
            return View(await torneoSolarContext.ToListAsync());
        }
        public async Task<IActionResult> Index1()
        {
            var torneoSolarContext = _context.Jugadores.Include(j => j.Equipo);
            return View(await torneoSolarContext.ToListAsync());
        }
        public async Task<IActionResult> Details1(int? id)
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
        // GET: Jugadores/Details/5
        [HttpGet]
        public async Task<IActionResult> GetJugadorDetails(int id)
        {
            var jugadore = await _context.Jugadores
                .Include(j => j.Equipo)
                .FirstOrDefaultAsync(m => m.JugadorId == id);

            if (jugadore == null)
            {
                return NotFound();
            }

            return Json(new
            {
                jugadore.Nombre,
                jugadore.FechaNacimiento,
                jugadore.Identificacion,
                jugadore.Peso,
                jugadore.Altura,
                jugadore.Posicion,
                Equipo = jugadore.Equipo?.Nombre
            });
        }


        // GET: Jugadores/Create
        public IActionResult Create()
        {
            ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre");
            return View();
        }

        // POST: Jugadores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JugadorId,Nombre,FechaNacimiento,Identificacion,Peso,Altura,Posicion,EquipoId")] Jugadore jugadore, IFormFile foto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Si se proporciona una imagen, se guarda en el servidor
                    if (foto != null && foto.Length > 0)
                    {
                        var fileName = $"{jugadore.Nombre}.jpeg";  // Usar el nombre del jugador como nombre del archivo
                        var filePath = Path.Combine(_uploadPath, fileName);

                        // Crear el directorio si no existe
                        if (!Directory.Exists(_uploadPath))
                        {
                            Directory.CreateDirectory(_uploadPath);
                        }

                        // Guardar la imagen en el servidor
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await foto.CopyToAsync(stream);
                        }

                        jugadore.Foto = $"/images/jugadores/{fileName}";  // Guardar la ruta relativa en la base de datos
                    }

                    _context.Add(jugadore);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", jugadore.EquipoId);
                return View(jugadore);
            }
            catch (Exception ex)
            {
                // Proporcionar un mensaje de error detallado
                ModelState.AddModelError(string.Empty, $"Error al crear el jugador: {ex.Message}");
                ViewData["EquipoId"] = new SelectList(_context.Equipos, "EquipoId", "Nombre", jugadore.EquipoId);
                return View(jugadore);
            }
        }

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

        // GET: Jugadores/Delete/5
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

        // POST: Jugadores/Delete/5
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

