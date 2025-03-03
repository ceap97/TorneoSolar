namespace TorneoSolar.Models
{
    public partial class TablaPosiciones
    {
        public int Id { get; set; }
        public int EquipoId { get; set; }
        public int PJ { get; set; } = 0; // Partidos Jugados
        public int PG { get; set; } = 0; // Partidos Ganados
        public int PP { get; set; } = 0; // Partidos Perdidos
        public int Puntos { get; set; } = 0; // Puntos acumulados (por victoria o participación)
        public int PtsFavor { get; set; } = 0; // Puntos anotados (canastas a favor)
        public int PtsContra { get; set; } = 0; // Puntos recibidos (canastas en contra)
        public int Diferencia { get; set; } // Columna calculada (PtsFavor - PtsContra)

        public virtual Equipo Equipo { get; set; } = null!;
    }

}
