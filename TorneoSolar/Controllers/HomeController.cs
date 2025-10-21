using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TorneoSolar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TorneoSolar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TorneoSolarContext _context;

        public HomeController(ILogger<HomeController> logger, TorneoSolarContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Incrementar el contador de visitantes
                var visitorCount = await _context.VisitorCounts.FirstOrDefaultAsync();
                if (visitorCount != null)
                {
                    visitorCount.Count++;
                    _context.Update(visitorCount);
                    await _context.SaveChangesAsync();
                }

                var noticias = await _context.Noticias.ToListAsync();
                var ultimosResultados = await _context.Partidos
                    .Include(p => p.LocalEquipo)
                    .Include(p => p.VisitanteEquipo)
                    .Include(p => p.ResultadosPartido)
                    .OrderByDescending(p => p.FechaHora)
                    .Take(5)
                    .ToListAsync();

                var viewModel = new HomeViewModel
                {
                    Noticias = noticias,
                    UltimosResultados = ultimosResultados
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Home/Index");
                return RedirectToAction("Error");
            }
        }
        [Authorize]
        public IActionResult Admin()
        {
            return View();
        }
        public async Task<IActionResult> CerrarSesion()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión");
                return RedirectToAction("Error");
            }
        }
        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar Privacy");
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
