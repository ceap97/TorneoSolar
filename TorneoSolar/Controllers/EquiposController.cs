using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
        private const string FromEmail = "solarbaclub@gmail.com";
        private const string FromName = "Torneo Solar";
        private const string FromPassword = "wldc phxl eski qkyd\r\n"; // Usa configuración segura en producción


        public EquiposController(TorneoSolarContext context)
        {
            _context = context;
        }
        [Authorize]

        [HttpPost]
        // Método privado para enviar correos
        private async Task<bool> EnviarCorreoAsync(string destinatario, string nombreDestinatario, string asunto, string cuerpo)
        {
            try
            {
                var fromAddress = new MailAddress(FromEmail, FromName);
                var toAddress = new MailAddress(destinatario, nombreDestinatario);

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(FromEmail, FromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true
                })
                {
                    await smtp.SendMailAsync(message);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        [Authorize]

        [HttpPost]
        public async Task<IActionResult> DenegarSolicitud(int id)
        {
            var solicitud = await _context.SolicitudesEquipos.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            // Determinar el motivo del rechazo
            string motivo;
            if (string.IsNullOrEmpty(solicitud.Planilla) && string.IsNullOrEmpty(solicitud.Logo))
            {
                motivo = "Tu solicitud fue rechazada porque no adjuntaste ni el logo ni la planilla del equipo. Por favor, asegúrate de adjuntar ambos archivos al volver a intentarlo.";
            }
            else
            {
                motivo = "Tu solicitud fue rechazada. Por favor, revisa los datos y vuelve a intentarlo.";
            }

            // Enviar correo de notificación
            if (!string.IsNullOrEmpty(solicitud.Correo))
            {
                var subject = "Solicitud de registro de equipo denegada";
                var body = $@"
                    <p>Hola {solicitud.NombreEncargado},</p>
                    <p>{motivo}</p>
                    <p>Equipo: <strong>{solicitud.Nombre}</strong></p>
                    <p>Ciudad: <strong>{solicitud.Ciudad}</strong></p>
                    <p>Fecha de solicitud: <strong>{solicitud.FechaSolicitud?.ToString("g")}</strong></p>
                    <p>Atentamente,<br/>El equipo de Torneo Solar</p>
                ";

                await EnviarCorreoAsync(solicitud.Correo, solicitud.NombreEncargado, subject, body);
            }

            // Eliminar la solicitud
            _context.SolicitudesEquipos.Remove(solicitud);
            await _context.SaveChangesAsync();

            TempData["Aprobado"] = "La solicitud fue denegada y se notificó al solicitante.";
            return RedirectToAction(nameof(Solicitudes));
        }



        // GET: Equipos/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Equipos/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(SolicitudEquipo solicitud, IFormFile logo, IFormFile planilla)
        {
            if (ModelState.IsValid)
            {
                // Guardar logo si se sube
                if (logo != null && logo.Length > 0)
                {
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/solicitudes");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    var fileName = $"{solicitud.Nombre}_{DateTime.Now.Ticks}{Path.GetExtension(logo.FileName)}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await logo.CopyToAsync(stream);
                    }
                    solicitud.Logo = $"/images/solicitudes/{fileName}";
                }

                // Guardar planilla si se sube
                if (planilla != null && planilla.Length > 0)
                {
                    var planillaPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/planillas/solicitudes");
                    if (!Directory.Exists(planillaPath))
                        Directory.CreateDirectory(planillaPath);

                    var planillaFileName = $"{solicitud.Nombre}_{DateTime.Now.Ticks}{Path.GetExtension(planilla.FileName)}";
                    var planillaFilePath = Path.Combine(planillaPath, planillaFileName);

                    using (var stream = new FileStream(planillaFilePath, FileMode.Create))
                    {
                        await planilla.CopyToAsync(stream);
                    }
                    solicitud.Planilla = $"/planillas/solicitudes/{planillaFileName}";
                }

                solicitud.FechaSolicitud = DateTime.Now;
                solicitud.Aprobada = false;

                _context.SolicitudesEquipos.Add(solicitud);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "¡Solicitud enviada! Un administrador revisará tu registro.";
                return RedirectToAction("Registro");
            }
            return View(solicitud);
        }


        // GET: Equipos/Solicitudes
        [Authorize]
        public async Task<IActionResult> Solicitudes()
        {
            var solicitudes = await _context.SolicitudesEquipos
                .Where(s => !s.Aprobada)
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();
            return View(solicitudes);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AprobarSolicitud(int id)
        {
            var solicitud = await _context.SolicitudesEquipos.FindAsync(id);
            if (solicitud == null) return NotFound();

            // Crear el equipo real
            var equipo = new Equipo
            {
                Nombre = solicitud.Nombre,
                Ciudad = solicitud.Ciudad,
                Logo = solicitud.Logo
            };
            _context.Equipos.Add(equipo);

            solicitud.Aprobada = true;
            _context.SolicitudesEquipos.Update(solicitud);

            await _context.SaveChangesAsync();

            // Enviar correo de notificación
            try
            {
                var fromAddress = new MailAddress("solarbaclub@gmail.com", "Torneo Solar");
                var toAddress = new MailAddress(solicitud.Correo, solicitud.NombreEncargado);
                const string fromPassword = "wldc phxl eski qkyd\r\n"; // Usa configuración segura en producción
                const string subject = "¡Tu equipo ha sido aprobado!";
                string body = $"Hola {solicitud.NombreEncargado},\n\n" +
                              $"Tu equipo \"{solicitud.Nombre}\" ha sido aprobado para participar en el Torneo Solar.\n\n" +
                              $"¡Bienvenido!\n\n" +
                              $"Saludos,\nTorneo Solar";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("solarbaclub@gmail.com", fromPassword) // El usuario es el correo completo
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                TempData["Aprobado"] = "El equipo ha sido aprobado, pero no se pudo enviar el correo: " + ex.Message;
                return RedirectToAction("Solicitudes");
            }

            TempData["Aprobado"] = "El equipo ha sido aprobado exitosamente y se notificó al correo del encargado.";
            return RedirectToAction("Solicitudes");
        }


        public async Task<IActionResult> Partidos(int equipoId)
        {
            var partidos = await _context.Partidos
                .Include(p => p.LocalEquipo)
                .Include(p => p.VisitanteEquipo)
                .Include(p => p.ResultadosPartido)
                .Where(p => p.LocalEquipoId == equipoId || p.VisitanteEquipoId == equipoId)
                .OrderByDescending(p => p.FechaHora)
                .ToListAsync();

            var equipo = await _context.Equipos.FindAsync(equipoId);
            ViewBag.EquipoNombre = equipo?.Nombre ?? "Equipo";

            return View(partidos);
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
                    return RedirectToAction(nameof(Index1));
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EquipoId,Nombre,Ciudad,Logo")] Equipo equipo, IFormFile logo)
        {
            if (id != equipo.EquipoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (logo != null)
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
                    }

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
                return RedirectToAction(nameof(Index1));
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
            return RedirectToAction(nameof(Index1));
        }

        private bool EquipoExists(int id)
        {
            return _context.Equipos.Any(e => e.EquipoId == id);
        }
    }
}
