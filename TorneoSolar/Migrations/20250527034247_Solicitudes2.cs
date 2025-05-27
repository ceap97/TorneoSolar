using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoSolar.Migrations
{
    /// <inheritdoc />
    public partial class Solicitudes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreEncargado",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Planilla",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "SolicitudesEquipos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correo",
                table: "SolicitudesEquipos");

            migrationBuilder.DropColumn(
                name: "NombreEncargado",
                table: "SolicitudesEquipos");

            migrationBuilder.DropColumn(
                name: "Planilla",
                table: "SolicitudesEquipos");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "SolicitudesEquipos");
        }
    }
}
