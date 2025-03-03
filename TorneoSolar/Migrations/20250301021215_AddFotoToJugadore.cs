using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoSolar.Migrations
{
    /// <inheritdoc />
    public partial class AddFotoToJugadore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Jugadores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Jugadores");
        }
    }
}
