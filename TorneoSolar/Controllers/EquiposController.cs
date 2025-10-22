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
using TorneoSolar.Options;
using Microsoft.Extensions.Options;

namespace TorneoSolar.Controllers
{
    public class EquiposController : Controller
    {
        private readonly TorneoSolarContext _context;
        private readonly ILogger<EquiposController> _logger;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/equipos");
        private readonly SmtpOptions _smtpOptions;

        public EquiposController(TorneoSolarContext context, ILogger<EquiposController> logger, IOptions<SmtpOptions> smtpOptions)
        {
            _context = context;
            _logger = logger;
            _smtpOptions = smtpOptions.Value;
        }
        [Authorize]
        // Método privado para enviar correos
        private async Task<bool> EnviarCorreoAsync(string destinatario, string nombreDestinatario, string asunto, string cuerpo)
        {
            try
            {
                var fromAddress = new MailAddress(_smtpOptions.FromEmail, _smtpOptions.FromName);
                var toAddress = new MailAddress(destinatario, nombreDestinatario);

                var smtp = new SmtpClient
                {
                    Host = _smtpOptions.Host,
                    Port = _smtpOptions.Port,
                    EnableSsl = _smtpOptions.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_smtpOptions.FromEmail, _smtpOptions.Password)
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
                // Registrar fallo de envío de correo, pero no interrumpir el flujo principal
                _logger.LogWarning("Fallo al enviar correo a {Destinatario} con asunto {Asunto}", destinatario, asunto);
                return false;
            }
        }
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
        public async Task<IActionResult> DenegarSolicitud(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al denegar solicitud {SolicitudId}", id);
                return RedirectToAction("Error", "Home");
            }
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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar solicitud de equipo {Nombre}", solicitud?.Nombre);
                TempData["Mensaje"] = "Ocurrió un error al procesar la solicitud. Intente nuevamente.";
                return RedirectToAction("Registro");
            }
        }


        // GET: Equipos/Solicitudes
        [Authorize]
        public async Task<IActionResult> Solicitudes()
        {
            try
            {
                var solicitudes = await _context.SolicitudesEquipos
                    .Where(s => !s.Aprobada)
                    .OrderByDescending(s => s.FechaSolicitud)
                    .ToListAsync();
                return View(solicitudes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener solicitudes de equipos");
                return RedirectToAction("Error", "Home");
            }
        }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
        public async Task<IActionResult> AprobarSolicitud(int id)
        {
            try
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
                    var subject = "¡Tu equipo ha sido aprobado!";
                    string body = $"Hola {solicitud.NombreEncargado},\n\n" +
                                  $"Tu equipo \"{solicitud.Nombre}\" ha sido aprobado para participar en el Torneo Solar.\n\n" +
                                  $"¡Bienvenido!\n\n" +
                                  $"Saludos,\nTorneo Solar";

                    await EnviarCorreoAsync(solicitud.Correo, solicitud.NombreEncargado, subject, body);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Aprobado equipo pero falló envío de correo para solicitud {SolicitudId}", id);
                    TempData["Aprobado"] = "El equipo ha sido aprobado, pero no se pudo enviar el correo.";
                    return RedirectToAction("Solicitudes");
                }

                TempData["Aprobado"] = "El equipo ha sido aprobado exitosamente y se notificó al correo del encargado.";
                return RedirectToAction("Solicitudes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al aprobar solicitud {SolicitudId}", id);
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> Partidos(int equipoId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener partidos del equipo {EquipoId}", equipoId);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Equipos
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Equipos.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Index de Equipos");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]
        public async Task<IActionResult> Index1()
        {
            try
            {
                return View(await _context.Equipos.ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Index1 de Equipos");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // GET: Equipos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalles del equipo {EquipoId}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // GET: Equipos/Create
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista Create de Equipos");
                return RedirectToAction("Error", "Home");
            }
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
                _logger.LogError(ioEx, "Error de IO al crear equipo {Nombre}", equipo?.Nombre);
                return BadRequest(new { message = "Error al guardar el archivo: " + ioEx.Message });
            }
            catch (Exception ex)
            {
                // Manejar otros tipos de errores
                _logger.LogError(ex, "Error general al crear equipo {Nombre}", equipo?.Nombre);
                return BadRequest(new { message = "Error al crear el equipo: " + ex.Message });
            }
        }


        [Authorize]
        // GET: Equipos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista Edit de equipo {EquipoId}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EquipoId,Nombre,Ciudad,Logo")] Equipo equipo, IFormFile logo)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar equipo {EquipoId}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]

        // GET: Equipos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista Delete de equipo {EquipoId}", id);
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var equipo = await _context.Equipos.FindAsync(id);
                if (equipo != null)
                {
                    _context.Equipos.Remove(equipo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index1));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar equipo {EquipoId}", id);
                return RedirectToAction("Error", "Home");
            }
        }

        private bool EquipoExists(int id)
        {
            return _context.Equipos.Any(e => e.EquipoId == id);
        }
    }
}
