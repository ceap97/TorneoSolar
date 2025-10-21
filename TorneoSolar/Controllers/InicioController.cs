using TorneoSolar.Models;
using TorneoSolar.Recursos;
using TorneoSolar.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TorneoSolar.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioServices _usuariosServicio;
        private readonly ILogger<InicioController> _logger;
        public InicioController(IUsuarioServices usuariosServicio, ILogger<InicioController> logger)
        {
            _usuariosServicio = usuariosServicio;
            _logger = logger;
        }
        [Authorize]
        public IActionResult Registrarse()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista Registrarse");
                return RedirectToAction("Error", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            try
            {
                modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);
                Usuario usuario_creado = await _usuariosServicio.SaveUsuario(modelo);
                if (usuario_creado.UsuarioId > 0)
                    return RedirectToAction("IniciarSesion", "Inicio");

                ViewData["Mensaje"] = "No se pudo crear el usuario";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario {Correo}", modelo?.Correo);
                ViewData["Mensaje"] = "Ocurrió un error al registrarse";
                return View();
            }

        }
        
            [HttpGet]
            public IActionResult IniciarSesion()
            {
                try
                {
                    return PartialView();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al cargar vista IniciarSesion");
                    return RedirectToAction("Error", "Home");
                }
            }

        [HttpPost]
        public async Task< IActionResult> IniciarSesion(string correo, string clave)
        {
            try
            {
                Usuario usuario_encontrado = await _usuariosServicio.GetUsuario(correo, Utilidades.EncriptarClave(clave));
                if (usuario_encontrado == null)
                {
                    ViewData["Mensaje"] = "No se encontraron coincidencias";
                    return View();
                }

                List<Claim>claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties
                    );
                return RedirectToAction("Admin","Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión para {Correo}", correo);
                ViewData["Mensaje"] = "Ocurrió un error al iniciar sesión";
                return View();
            }
        }
    }
}

