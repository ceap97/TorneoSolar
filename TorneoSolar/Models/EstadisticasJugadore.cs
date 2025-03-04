using System;
using System.Collections.Generic;

namespace TorneoSolar.Models;

public partial class EstadisticasJugadore
{
    public int EstadisticaId { get; set; }

    public int? JugadorId { get; set; }

    public int? PartidoId { get; set; }

    public int? Puntos { get; set; }

    public int? Rebotes { get; set; }

    public int? Asistencias { get; set; }

    public int? Bloqueos { get; set; }

    public int? Robos { get; set; }

    public int? MinutosJugados { get; set; }

    public virtual Jugadore Jugador { get; set; }

    public virtual Partido Partido { get; set; }
}
