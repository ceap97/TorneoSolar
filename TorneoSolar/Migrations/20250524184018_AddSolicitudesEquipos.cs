using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorneoSolar.Migrations
{
    /// <inheritdoc />
    public partial class AddSolicitudesEquipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "UQ__Jugadore__D6F931E56DFE6D8C",
            //    table: "Jugadores");

            //migrationBuilder.AlterColumn<string>(
            //    name: "NombreUsuario",
            //    table: "Usuario",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Correo",
            //    table: "Usuario",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Clave",
            //    table: "Usuario",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Nombre",
            //    table: "Jugadores",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Identificacion",
            //    table: "Jugadores",
            //    type: "nvarchar(20)",
            //    maxLength: 20,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(20)",
            //    oldMaxLength: 20);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Nombre",
            //    table: "Equipos",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Ciudad",
            //    table: "Equipos",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100);

            //migrationBuilder.CreateTable(
            //    name: "Noticias",
            //    columns: table => new
            //    {
            //        NoticiasId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
            //        Comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Noticias", x => x.NoticiasId);
            //    });

            migrationBuilder.CreateTable(
                name: "SolicitudesEquipos",
                columns: table => new
                {
                    SolicitudEquipoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aprobada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesEquipos", x => x.SolicitudEquipoId);
                });
        }

        //    migrationBuilder.CreateTable(
        //        name: "TablaPosiciones",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            EquipoId = table.Column<int>(type: "int", nullable: false),
        //            PJ = table.Column<int>(type: "int", nullable: false),
        //            PG = table.Column<int>(type: "int", nullable: false),
        //            PP = table.Column<int>(type: "int", nullable: false),
        //            Puntos = table.Column<int>(type: "int", nullable: false),
        //            PtsFavor = table.Column<int>(type: "int", nullable: false),
        //            PtsContra = table.Column<int>(type: "int", nullable: false),
        //            Diferencia = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_TablaPosiciones", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_TablaPosiciones_Equipos_EquipoId",
        //                column: x => x.EquipoId,
        //                principalTable: "Equipos",
        //                principalColumn: "EquipoID",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "TablaPosicionesFem",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            EquipoId = table.Column<int>(type: "int", nullable: false),
        //            PJ = table.Column<int>(type: "int", nullable: false),
        //            PG = table.Column<int>(type: "int", nullable: false),
        //            PP = table.Column<int>(type: "int", nullable: false),
        //            Puntos = table.Column<int>(type: "int", nullable: false),
        //            PtsFavor = table.Column<int>(type: "int", nullable: false),
        //            PtsContra = table.Column<int>(type: "int", nullable: false),
        //            Diferencia = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_TablaPosicionesFem", x => x.Id);
        //            table.ForeignKey(
        //                name: "FK_TablaPosicionesFem_Equipos_EquipoId",
        //                column: x => x.EquipoId,
        //                principalTable: "Equipos",
        //                principalColumn: "EquipoID",
        //                onDelete: ReferentialAction.Cascade);
        //        });

        //    migrationBuilder.CreateTable(
        //        name: "VisitorCount",
        //        columns: table => new
        //        {
        //            Id = table.Column<int>(type: "int", nullable: false)
        //                .Annotation("SqlServer:Identity", "1, 1"),
        //            Count = table.Column<int>(type: "int", nullable: false)
        //        },
        //        constraints: table =>
        //        {
        //            table.PrimaryKey("PK_VisitorCount", x => x.Id);
        //        });

        //    migrationBuilder.CreateIndex(
        //        name: "UQ__Jugadore__D6F931E56DFE6D8C",
        //        table: "Jugadores",
        //        column: "Identificacion",
        //        unique: true,
        //        filter: "[Identificacion] IS NOT NULL");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_TablaPosiciones_EquipoId",
        //        table: "TablaPosiciones",
        //        column: "EquipoId");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_TablaPosicionesFem_EquipoId",
        //        table: "TablaPosicionesFem",
        //        column: "EquipoId");
        //}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Noticias");

            migrationBuilder.DropTable(
                name: "SolicitudesEquipos");

            //migrationBuilder.DropTable(
            //    name: "TablaPosiciones");

            //migrationBuilder.DropTable(
            //    name: "TablaPosicionesFem");

            //migrationBuilder.DropTable(
            //    name: "VisitorCount");

            //migrationBuilder.DropIndex(
            //    name: "UQ__Jugadore__D6F931E56DFE6D8C",
            //    table: "Jugadores");

            //migrationBuilder.AlterColumn<string>(
            //    name: "NombreUsuario",
            //    table: "Usuario",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Correo",
            //    table: "Usuario",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Clave",
            //    table: "Usuario",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Nombre",
            //    table: "Jugadores",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Identificacion",
            //    table: "Jugadores",
            //    type: "nvarchar(20)",
            //    maxLength: 20,
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(20)",
            //    oldMaxLength: 20,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Nombre",
            //    table: "Equipos",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Ciudad",
            //    table: "Equipos",
            //    type: "nvarchar(100)",
            //    maxLength: 100,
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(100)",
            //    oldMaxLength: 100,
            //    oldNullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "UQ__Jugadore__D6F931E56DFE6D8C",
            //    table: "Jugadores",
            //    column: "Identificacion",
            //    unique: true);
        }
    }
}
