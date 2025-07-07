using System;

namespace TorneoSolar.Models;

    public partial class Bracket
    {
        public int BracketId { get; set; }

        public int PartidoId { get; set; }
        public int? ResultadoId { get; set; }
        public int EquipoLocalId { get; set; }
        public int EquipoVisitanteId { get; set; }
        public int Ronda { get; set; }
        public int? TablaPosicionesLocalId { get; set; }
        public int? TablaPosicionesVisitanteId { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual Partido Partido { get; set; }
        public virtual ResultadosPartido? Resultado { get; set; }
        public virtual Equipo EquipoLocal { get; set; }
        public virtual Equipo EquipoVisitante { get; set; }
        public virtual TablaPosiciones? TablaPosicionesLocal { get; set; }
        public virtual TablaPosiciones? TablaPosicionesVisitante { get; set; }
    }


