using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TorneoSolar.Models;
public class SolicitudEquipo
{
    public int SolicitudEquipoId { get; set; }
    [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
    public string Nombre { get; set; }
    
    public string Ciudad { get; set; }
    public string Logo { get; set; }
    public DateTime FechaSolicitud { get; set; } = DateTime.Now;
    public bool Aprobada { get; set; } = false;

    // Nuevos campos
    public string NombreEncargado { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string Planilla { get; set; } // Puede ser ruta de archivo o base64, según tu lógica
}

