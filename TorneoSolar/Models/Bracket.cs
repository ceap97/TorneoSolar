﻿#nullable enable
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

#pragma warning disable CS8632 // Anotaciones de referencia anulable fuera de contexto #nullable
    public virtual Partido Partido { get; set; } = null!;
        public virtual ResultadosPartido? Resultado { get; set; }
    public virtual Equipo EquipoLocal { get; set; } = null!;
    public virtual Equipo EquipoVisitante { get; set; } = null!;
        public virtual TablaPosiciones? TablaPosicionesLocal { get; set; }
        public virtual TablaPosiciones? TablaPosicionesVisitante { get; set; }
#pragma warning restore CS8632
    }


