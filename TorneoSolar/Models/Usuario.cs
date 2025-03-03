namespace TorneoSolar.Models
{

    public partial class Usuario
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public virtual ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
    }

}
