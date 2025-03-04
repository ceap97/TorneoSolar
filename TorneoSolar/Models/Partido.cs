using System;
using System.Collections.Generic;

namespace TorneoSolar.Models;

public partial class Partido
{
    public int PartidoId { get; set; }

    public DateTime FechaHora { get; set; }

    public int? LocalEquipoId { get; set; }

    public int? VisitanteEquipoId { get; set; }

    public string Ubicacion { get; set; }

    public virtual ICollection<EstadisticasJugadore> EstadisticasJugadores { get; set; } = new List<EstadisticasJugadore>();

    public virtual Equipo LocalEquipo { get; set; }

    public virtual ResultadosPartido ResultadosPartido { get; set; }

    public virtual Equipo VisitanteEquipo { get; set; }
}
