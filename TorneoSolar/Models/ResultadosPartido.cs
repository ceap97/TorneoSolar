using System;
using System.Collections.Generic;

namespace TorneoSolar.Models;

public partial class ResultadosPartido
{
    public int ResultadoId { get; set; }

    public int? PartidoId { get; set; }

    public int PuntosLocal { get; set; }

    public int PuntosVisitante { get; set; }

    public virtual Partido? Partido { get; set; }
}
