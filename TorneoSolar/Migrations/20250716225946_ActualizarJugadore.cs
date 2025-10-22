using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoSolar.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarJugadore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [Jugadores] DROP CONSTRAINT [UQ__Jugadore__D6F931E56DFE6D8C];");

            migrationBuilder.DropColumn(
                name: "Identificacion",
                table: "Jugadores");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Jugadores",
                newName: "EPS");

            migrationBuilder.RenameColumn(
                name: "Altura",
                table: "Jugadores",
                newName: "Estatura");

            migrationBuilder.AlterColumn<string>(
                name: "Foto",
                table: "Jugadores",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Jugadores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Jugadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Jugadores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroDocumento",
                table: "Jugadores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "Jugadores",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoDoc",
                table: "Jugadores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            // --- Limpieza de datos antes de crear el índice único ---
            migrationBuilder.Sql(@"
                UPDATE Jugadores
                SET NumeroDocumento = CONCAT('TEMP_', JugadorId)
                WHERE NumeroDocumento = '';
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_NumeroDocumento",
                table: "Jugadores",
                column: "NumeroDocumento",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Jugadores_NumeroDocumento",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "NumeroDocumento",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "TipoDoc",
                table: "Jugadores");

            migrationBuilder.RenameColumn(
                name: "Estatura",
                table: "Jugadores",
                newName: "Altura");

            migrationBuilder.RenameColumn(
                name: "EPS",
                table: "Jugadores",
                newName: "Nombre");

            migrationBuilder.AlterColumn<string>(
                name: "Foto",
                table: "Jugadores",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identificacion",
                table: "Jugadores",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Jugadore__D6F931E56DFE6D8C",
                table: "Jugadores",
                column: "Identificacion",
                unique: true,
                filter: "[Identificacion] IS NOT NULL");
        }
    }
}
