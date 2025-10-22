using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TorneoSolar.Models;

[Table("Jugadores")]
public partial class Jugadore
{
    public int JugadorId { get; set; }

    // En esquema actual, la columna 'Nombre' fue renombrada a 'EPS'.
    // Mapeamos la propiedad existente 'Nombre' a la columna 'EPS' para mantener compatibilidad.
    [Column("EPS")]
    public string Nombre { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    // La columna 'Identificacion' fue reemplazada por 'NumeroDocumento'.
    [Column("NumeroDocumento")]
    public string Identificacion { get; set; } = null!;

    public decimal? Peso { get; set; }

    // La columna 'Altura' fue renombrada a 'Estatura'.
    [Column("Estatura")]
    public int? Altura { get; set; }

    public string Posicion { get; set; }
    public string Foto { get; set; }

    public int? EquipoId { get; set; }

    public virtual Equipo Equipo { get; set; }

    public virtual ICollection<EstadisticasJugadore> EstadisticasJugadores { get; set; } = new List<EstadisticasJugadore>();
}
