using System;
using System.ComponentModel.DataAnnotations;

namespace TorneoSolar.Models
{
    public class Noticias
    {
        [Key]
        public int NoticiasId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Titulo { get; set; }

        public string? Comentario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public string? Imagen { get; set; }
    }
}
