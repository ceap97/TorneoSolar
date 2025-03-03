using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoSolar.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipoUsuario");

            // Agregar la columna 'UsuarioId' a la tabla 'Equipos'
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Equipos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_UsuarioId",
                table: "Equipos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Usuario_UsuarioId",
                table: "Equipos",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Usuario_UsuarioId",
                table: "Equipos");

            migrationBuilder.DropIndex(
                name: "IX_Equipos_UsuarioId",
                table: "Equipos");

            // Eliminar la columna 'UsuarioId' de la tabla 'Equipos'
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Equipos");

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

            migrationBuilder.CreateIndex(
                name: "IX_EquipoUsuario_UsuariosUsuarioId",
                table: "EquipoUsuario",
                column: "UsuariosUsuarioId");
        }
    }
}
