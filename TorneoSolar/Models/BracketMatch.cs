using System;

namespace TorneoSolar.Models;
public class BracketMatch
{
    public int Ronda { get; set; }
    public string EquipoLocal { get; set; }
    public string EquipoVisitante { get; set; }
    public int? PuntosLocal { get; set; }
    public int? PuntosVisitante { get; set; }
    public int? PosicionLocal { get; set; }
    public int? PosicionVisitante { get; set; }
}
