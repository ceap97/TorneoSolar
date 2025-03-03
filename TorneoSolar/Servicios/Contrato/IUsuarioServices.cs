using Microsoft.EntityFrameworkCore;
using TorneoSolar;
using TorneoSolar.Models;



namespace TorneoSolar.Servicios.Contrato
{
    public interface IUsuarioServices
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario modelo);  
    }
}
