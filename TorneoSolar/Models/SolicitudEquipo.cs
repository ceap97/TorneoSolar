using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TorneoSolar.Models;
public class SolicitudEquipo
{
    public int SolicitudEquipoId { get; set; }

    [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La ciudad es obligatoria.")]
    public string Ciudad { get; set; }

    // El logo se maneja como archivo en el formulario, pero si quieres forzar que se suba, valida en el controlador.
    public string Logo { get; set; }

    public DateTime? FechaSolicitud { get; set; } = DateTime.Now;
    public bool Aprobada { get; set; } = false;

    [Required(ErrorMessage = "El nombre del encargado es obligatorio.")]
    public string NombreEncargado { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    public string Telefono { get; set; }

    // La planilla se maneja como archivo en el formulario, pero si quieres forzar que se suba, valida en el controlador.
    public string Planilla { get; set; }
}
