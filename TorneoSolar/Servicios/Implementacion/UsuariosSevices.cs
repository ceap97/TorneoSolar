using Microsoft.EntityFrameworkCore;
using TorneoSolar.Servicios.Contrato;
using TorneoSolar.Models;

namespace TorneoSolar.Servicios.Implementacion
{
    public class UsuariosSevices : IUsuarioServices
    {

        private readonly TorneoSolarContext _dbContext;
        public UsuariosSevices(TorneoSolarContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuario_encontrado = await _dbContext.Usuario.Where(u => u.Correo == correo && u.Clave == clave)
                .FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            _dbContext.Usuario.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
