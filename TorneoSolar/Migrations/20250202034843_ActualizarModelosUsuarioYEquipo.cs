using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoSolar.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModelosUsuarioYEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Equipos",
            //    columns: table => new
            //    {
            //        EquipoID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UsuarioId = table.Column<int>(type: "int", nullable: true),
            //        Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Ciudad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Logo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Equipos__DE8A0BFFE8D35577", x => x.EquipoID);
            //    });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                });

            //migrationBuilder.CreateTable(
            //    name: "Jugadores",
            //    columns: table => new
            //    {
            //        JugadorID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
            //        Identificacion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            //        Peso = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
            //        Altura = table.Column<int>(type: "int", nullable: true),
            //        Posicion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        EquipoID = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Jugadore__4B5752421E42568D", x => x.JugadorID);
            //        table.ForeignKey(
            //            name: "FK__Jugadores__Equip__4CA06362",
            //            column: x => x.EquipoID,
            //            principalTable: "Equipos",
            //            principalColumn: "EquipoID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Partidos",
            //    columns: table => new
            //    {
            //        PartidoID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FechaHora = table.Column<DateTime>(type: "datetime", nullable: false),
            //        LocalEquipoID = table.Column<int>(type: "int", nullable: true),
            //        VisitanteEquipoID = table.Column<int>(type: "int", nullable: true),
            //        Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Partidos__DBC2E8D640879305", x => x.PartidoID);
            //        table.ForeignKey(
            //            name: "FK__Partidos__LocalE__4F7CD00D",
            //            column: x => x.LocalEquipoID,
            //            principalTable: "Equipos",
            //            principalColumn: "EquipoID");
            //        table.ForeignKey(
            //            name: "FK__Partidos__Visita__5070F446",
            //            column: x => x.VisitanteEquipoID,
            //            principalTable: "Equipos",
            //            principalColumn: "EquipoID");
            //    });

            migrationBuilder.CreateTable(
                name: "EquipoUsuario",
                columns: table => new
                {
                    EquiposEquipoId = table.Column<int>(type: "int", nullable: false),
                    UsuariosUsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipoUsuario", x => new { x.EquiposEquipoId, x.UsuariosUsuarioId });
                    table.ForeignKey(
                        name: "FK_EquipoUsuario_Equipos_EquiposEquipoId",
                        column: x => x.EquiposEquipoId,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipoUsuario_Usuario_UsuariosUsuarioId",
                        column: x => x.UsuariosUsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "EstadisticasJugadores",
            //    columns: table => new
            //    {
            //        EstadisticaID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        JugadorID = table.Column<int>(type: "int", nullable: true),
            //        PartidoID = table.Column<int>(type: "int", nullable: true),
            //        Puntos = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        Rebotes = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        Asistencias = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        Bloqueos = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        Robos = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
            //        MinutosJugados = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Estadist__5E78B5EC933DC896", x => x.EstadisticaID);
            //        table.ForeignKey(
            //            name: "FK__Estadisti__Jugad__571DF1D5",
            //            column: x => x.JugadorID,
            //            principalTable: "Jugadores",
            //            principalColumn: "JugadorID");
            //        table.ForeignKey(
            //            name: "FK__Estadisti__Parti__5812160E",
            //            column: x => x.PartidoID,
            //            principalTable: "Partidos",
            //            principalColumn: "PartidoID");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ResultadosPartidos",
            //    columns: table => new
            //    {
            //        ResultadoID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PartidoID = table.Column<int>(type: "int", nullable: true),
            //        PuntosLocal = table.Column<int>(type: "int", nullable: false),
            //        PuntosVisitante = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Resultad__7904DD4125ECEB63", x => x.ResultadoID);
            //        table.ForeignKey(
            //            name: "FK__Resultado__Parti__5441852A",
            //            column: x => x.PartidoID,
            //            principalTable: "Partidos",
            //            principalColumn: "PartidoID");
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_EquipoUsuario_UsuariosUsuarioId",
                table: "EquipoUsuario",
                column: "UsuariosUsuarioId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EstadisticasJugadores_JugadorID",
            //    table: "EstadisticasJugadores",
            //    column: "JugadorID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EstadisticasJugadores_PartidoID",
            //    table: "EstadisticasJugadores",
            //    column: "PartidoID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Jugadores_EquipoID",
            //    table: "Jugadores",
            //    column: "EquipoID");

            //migrationBuilder.CreateIndex(
            //    name: "UQ__Jugadore__D6F931E56DFE6D8C",
            //    table: "Jugadores",
            //    column: "Identificacion",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Partidos_LocalEquipoID",
            //    table: "Partidos",
            //    column: "LocalEquipoID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Partidos_VisitanteEquipoID",
            //    table: "Partidos",
            //    column: "VisitanteEquipoID");

            //migrationBuilder.CreateIndex(
            //    name: "UQ__Resultad__DBC2E8D77132EFE8",
            //    table: "ResultadosPartidos",
            //    column: "PartidoID",
            //    unique: true,
            //    filter: "[PartidoID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipoUsuario");

            //migrationBuilder.DropTable(
            //    name: "EstadisticasJugadores");

            //migrationBuilder.DropTable(
            //    name: "ResultadosPartidos");

            migrationBuilder.DropTable(
                name: "Usuario");

            //migrationBuilder.DropTable(
            //    name: "Jugadores");

            //migrationBuilder.DropTable(
            //    name: "Partidos");

            //migrationBuilder.DropTable(
            //    name: "Equipos");
        }
    }
}
