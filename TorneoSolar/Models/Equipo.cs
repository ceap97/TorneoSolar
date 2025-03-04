using System;
using System.Collections.Generic;

namespace TorneoSolar.Models;

public partial class Equipo
{
    public int EquipoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Logo { get; set; }

    public virtual ICollection<Jugadore> Jugadores { get; set; } = new List<Jugadore>();

    public virtual ICollection<Partido> PartidoLocalEquipos { get; set; } = new List<Partido>();

    public virtual ICollection<Partido> PartidoVisitanteEquipos { get; set; } = new List<Partido>();
}
