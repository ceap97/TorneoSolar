using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoSolar.Migrations
{
    /// <inheritdoc />
    public partial class BracketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NombreEncargado",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaSolicitud",
                table: "SolicitudesEquipos",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ciudad",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Brackets",
                columns: table => new
                {
                    BracketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartidoId = table.Column<int>(type: "int", nullable: false),
                    ResultadoId = table.Column<int>(type: "int", nullable: true),
                    EquipoLocalId = table.Column<int>(type: "int", nullable: false),
                    EquipoVisitanteId = table.Column<int>(type: "int", nullable: false),
                    Ronda = table.Column<int>(type: "int", nullable: false),
                    TablaPosicionesLocalId = table.Column<int>(type: "int", nullable: true),
                    TablaPosicionesVisitanteId = table.Column<int>(type: "int", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brackets", x => x.BracketId);
                    table.ForeignKey(
                        name: "FK_Brackets_Equipos_EquipoLocalId",
                        column: x => x.EquipoLocalId,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brackets_Equipos_EquipoVisitanteId",
                        column: x => x.EquipoVisitanteId,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brackets_Partidos_PartidoId",
                        column: x => x.PartidoId,
                        principalTable: "Partidos",
                        principalColumn: "PartidoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brackets_ResultadosPartidos_ResultadoId",
                        column: x => x.ResultadoId,
                        principalTable: "ResultadosPartidos",
                        principalColumn: "ResultadoID");
                    table.ForeignKey(
                        name: "FK_Brackets_TablaPosiciones_TablaPosicionesLocalId",
                        column: x => x.TablaPosicionesLocalId,
                        principalTable: "TablaPosiciones",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brackets_TablaPosiciones_TablaPosicionesVisitanteId",
                        column: x => x.TablaPosicionesVisitanteId,
                        principalTable: "TablaPosiciones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_EquipoLocalId",
                table: "Brackets",
                column: "EquipoLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_EquipoVisitanteId",
                table: "Brackets",
                column: "EquipoVisitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_PartidoId",
                table: "Brackets",
                column: "PartidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_ResultadoId",
                table: "Brackets",
                column: "ResultadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_TablaPosicionesLocalId",
                table: "Brackets",
                column: "TablaPosicionesLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_TablaPosicionesVisitanteId",
                table: "Brackets",
                column: "TablaPosicionesVisitanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brackets");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NombreEncargado",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaSolicitud",
                table: "SolicitudesEquipos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Ciudad",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
