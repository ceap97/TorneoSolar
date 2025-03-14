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
    public class EquiposController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/equipos");

        public EquiposController(TorneoSolarContext context)
        {
            _context = context;
        }
        // GET: Equipos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Equipos.ToListAsync());
        }
        [Authorize]
        public async Task<IActionResult> Index1()
        {
            return View(await _context.Equipos.ToListAsync());
        }
        [Authorize]

        // GET: Equipos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(m => m.EquipoId == id);
            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }
        [Authorize]

        // GET: Equipos/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EquipoId,Nombre,Ciudad,Logo")] Equipo equipo, IFormFile logo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fileName = $"{equipo.Nombre}.jpeg";  // Usar el nombre del equipo como nombre del archivo
                    var filePath = Path.Combine(_uploadPath, fileName);

                    // Crear el directorio si no existe
                    if (!Directory.Exists(_uploadPath))
                    {
                        Directory.CreateDirectory(_uploadPath);
                    }

                    // Guardar la imagen en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await logo.CopyToAsync(stream);
                    }

                    equipo.Logo = $"/images/equipos/{fileName}";  // Guardar la ruta relativa en la base de datos

                    _context.Add(equipo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(equipo);
            }
            catch (IOException ioEx)
            {
                // Manejar errores específicos de IO
                return BadRequest(new { message = "Error al guardar el archivo: " + ioEx.Message });
            }
            catch (Exception ex)
            {
                // Manejar otros tipos de errores
                return BadRequest(new { message = "Error al crear el equipo: " + ex.Message });
            }
        }


        [Authorize]
        // GET: Equipos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }
            return View(equipo);
        }

        // POST: Equipos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EquipoId,Nombre,Ciudad,Logo")] Equipo equipo)
        {
            if (id != equipo.EquipoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipoExists(equipo.EquipoId))
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
            return View(equipo);
        }
        [Authorize]

        // GET: Equipos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(m => m.EquipoId == id);
            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }
        [Authorize]

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo != null)
            {
                _context.Equipos.Remove(equipo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipoExists(int id)
        {
            return _context.Equipos.Any(e => e.EquipoId == id);
        }
    }
}
