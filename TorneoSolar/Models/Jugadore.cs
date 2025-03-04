using System;
using System.Collections.Generic;

namespace TorneoSolar.Models;

public partial class Jugadore
{
    public int JugadorId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Identificacion { get; set; } = null!;

    public decimal? Peso { get; set; }

    public int? Altura { get; set; }

    public string Posicion { get; set; }
    public string Foto { get; set; }

    public int? EquipoId { get; set; }

    public virtual Equipo Equipo { get; set; }

    public virtual ICollection<EstadisticasJugadore> EstadisticasJugadores { get; set; } = new List<EstadisticasJugadore>();
}
