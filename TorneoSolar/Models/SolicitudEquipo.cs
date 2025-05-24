using System;
using System.Collections.Generic;

namespace TorneoSolar.Models;
public class SolicitudEquipo
{
    public int SolicitudEquipoId { get; set; }
    public string Nombre { get; set; }
    public string Ciudad { get; set; }
    public string Logo { get; set; }
    public DateTime FechaSolicitud { get; set; } = DateTime.Now;
    public bool Aprobada { get; set; } = false;
}
