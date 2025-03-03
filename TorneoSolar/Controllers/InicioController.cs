using TorneoSolar.Models;
using TorneoSolar.Recursos;
using TorneoSolar.Servicios.Contrato;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace TorneoSolar.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioServices _usuariosServicio;
        public InicioController(IUsuarioServices usuariosServicio)
        {
            _usuariosServicio = usuariosServicio;
        }
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);
            Usuario usuario_creado = await _usuariosServicio.SaveUsuario(modelo);
            if (usuario_creado.UsuarioId > 0)
                return RedirectToAction("IniciarSesion", "Inicio");

            ViewData["Mensaje"] = "No se pudo crear el usuario";
            return View();

        }
        
            [HttpGet]
            public IActionResult IniciarSesion()
            {
                return PartialView();
            }

        [HttpPost]
        public async Task< IActionResult> IniciarSesion(string correo, string clave)
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
    }
}

