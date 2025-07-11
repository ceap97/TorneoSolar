﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TorneoSolar.Models;

#nullable disable

namespace TorneoSolar.Migrations
{
    [DbContext(typeof(TorneoSolarContext))]
    [Migration("20250704215502_BracketTable")]
    partial class BracketTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TorneoSolar.Models.Bracket", b =>
                {
                    b.Property<int>("BracketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BracketId"));

                    b.Property<int>("EquipoLocalId")
                        .HasColumnType("int");

                    b.Property<int>("EquipoVisitanteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("PartidoId")
                        .HasColumnType("int");

                    b.Property<int?>("ResultadoId")
                        .HasColumnType("int");

                    b.Property<int>("Ronda")
                        .HasColumnType("int");

                    b.Property<int?>("TablaPosicionesLocalId")
                        .HasColumnType("int");

                    b.Property<int?>("TablaPosicionesVisitanteId")
                        .HasColumnType("int");

                    b.HasKey("BracketId");

                    b.HasIndex("EquipoLocalId");

                    b.HasIndex("EquipoVisitanteId");

                    b.HasIndex("PartidoId");

                    b.HasIndex("ResultadoId");

                    b.HasIndex("TablaPosicionesLocalId");

                    b.HasIndex("TablaPosicionesVisitanteId");

                    b.ToTable("Brackets");
                });

            modelBuilder.Entity("TorneoSolar.Models.Equipo", b =>
                {
                    b.Property<int>("EquipoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EquipoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EquipoId"));

                    b.Property<string>("Ciudad")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Logo")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("EquipoId")
                        .HasName("PK__Equipos__DE8A0BFFE8D35577");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Equipos");
                });

            modelBuilder.Entity("TorneoSolar.Models.EstadisticasJugadore", b =>
                {
                    b.Property<int>("EstadisticaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EstadisticaID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstadisticaId"));

                    b.Property<int?>("Asistencias")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("Bloqueos")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("JugadorId")
                        .HasColumnType("int")
                        .HasColumnName("JugadorID");

                    b.Property<int?>("MinutosJugados")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("PartidoId")
                        .HasColumnType("int")
                        .HasColumnName("PartidoID");

                    b.Property<int?>("Puntos")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("Rebotes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("Robos")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("EstadisticaId")
                        .HasName("PK__Estadist__5E78B5EC933DC896");

                    b.HasIndex("JugadorId");

                    b.HasIndex("PartidoId");

                    b.ToTable("EstadisticasJugadores");
                });

            modelBuilder.Entity("TorneoSolar.Models.Jugadore", b =>
                {
                    b.Property<int>("JugadorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("JugadorID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JugadorId"));

                    b.Property<int?>("Altura")
                        .HasColumnType("int");

                    b.Property<int?>("EquipoId")
                        .HasColumnType("int")
                        .HasColumnName("EquipoID");

                    b.Property<DateOnly>("FechaNacimiento")
                        .HasColumnType("date");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identificacion")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal?>("Peso")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("Posicion")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("JugadorId")
                        .HasName("PK__Jugadore__4B5752421E42568D");

                    b.HasIndex("EquipoId");

                    b.HasIndex(new[] { "Identificacion" }, "UQ__Jugadore__D6F931E56DFE6D8C")
                        .IsUnique()
                        .HasFilter("[Identificacion] IS NOT NULL");

                    b.ToTable("Jugadores");
                });

            modelBuilder.Entity("TorneoSolar.Models.Noticias", b =>
                {
                    b.Property<int>("NoticiasId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NoticiasId"));

                    b.Property<string>("Comentario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("NoticiasId");

                    b.ToTable("Noticias");
                });

            modelBuilder.Entity("TorneoSolar.Models.Partido", b =>
                {
                    b.Property<int>("PartidoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PartidoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PartidoId"));

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("datetime");

                    b.Property<int?>("LocalEquipoId")
                        .HasColumnType("int")
                        .HasColumnName("LocalEquipoID");

                    b.Property<string>("Ubicacion")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("VisitanteEquipoId")
                        .HasColumnType("int")
                        .HasColumnName("VisitanteEquipoID");

                    b.HasKey("PartidoId")
                        .HasName("PK__Partidos__DBC2E8D640879305");

                    b.HasIndex("LocalEquipoId");

                    b.HasIndex("VisitanteEquipoId");

                    b.ToTable("Partidos");
                });

            modelBuilder.Entity("TorneoSolar.Models.ResultadosPartido", b =>
                {
                    b.Property<int>("ResultadoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ResultadoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResultadoId"));

                    b.Property<int?>("PartidoId")
                        .HasColumnType("int")
                        .HasColumnName("PartidoID");

                    b.Property<int>("PuntosLocal")
                        .HasColumnType("int");

                    b.Property<int>("PuntosVisitante")
                        .HasColumnType("int");

                    b.HasKey("ResultadoId")
                        .HasName("PK__Resultad__7904DD4125ECEB63");

                    b.HasIndex(new[] { "PartidoId" }, "UQ__Resultad__DBC2E8D77132EFE8")
                        .IsUnique()
                        .HasFilter("[PartidoID] IS NOT NULL");

                    b.ToTable("ResultadosPartidos");
                });

            modelBuilder.Entity("TorneoSolar.Models.SolicitudEquipo", b =>
                {
                    b.Property<int>("SolicitudEquipoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SolicitudEquipoId"));

                    b.Property<bool>("Aprobada")
                        .HasColumnType("bit");

                    b.Property<string>("Ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaSolicitud")
                        .HasColumnType("datetime2");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreEncargado")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Planilla")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SolicitudEquipoId");

                    b.ToTable("SolicitudesEquipos");
                });

            modelBuilder.Entity("TorneoSolar.Models.TablaPosiciones", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Diferencia")
                        .HasColumnType("int");

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.Property<int>("PG")
                        .HasColumnType("int");

                    b.Property<int>("PJ")
                        .HasColumnType("int");

                    b.Property<int>("PP")
                        .HasColumnType("int");

                    b.Property<int>("PtsContra")
                        .HasColumnType("int");

                    b.Property<int>("PtsFavor")
                        .HasColumnType("int");

                    b.Property<int>("Puntos")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EquipoId");

                    b.ToTable("TablaPosiciones");
                });

            modelBuilder.Entity("TorneoSolar.Models.TablaPosicionesFem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Diferencia")
                        .HasColumnType("int");

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.Property<int>("PG")
                        .HasColumnType("int");

                    b.Property<int>("PJ")
                        .HasColumnType("int");

                    b.Property<int>("PP")
                        .HasColumnType("int");

                    b.Property<int>("PtsContra")
                        .HasColumnType("int");

                    b.Property<int>("PtsFavor")
                        .HasColumnType("int");

                    b.Property<int>("Puntos")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EquipoId");

                    b.ToTable("TablaPosicionesFem");
                });

            modelBuilder.Entity("TorneoSolar.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioId"));

                    b.Property<string>("Clave")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Correo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreUsuario")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("TorneoSolar.Models.VisitorCount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VisitorCount");
                });

            modelBuilder.Entity("TorneoSolar.Models.Bracket", b =>
                {
                    b.HasOne("TorneoSolar.Models.Equipo", "EquipoLocal")
                        .WithMany()
                        .HasForeignKey("EquipoLocalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TorneoSolar.Models.Equipo", "EquipoVisitante")
                        .WithMany()
                        .HasForeignKey("EquipoVisitanteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TorneoSolar.Models.Partido", "Partido")
                        .WithMany()
                        .HasForeignKey("PartidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TorneoSolar.Models.ResultadosPartido", "Resultado")
                        .WithMany()
                        .HasForeignKey("ResultadoId");

                    b.HasOne("TorneoSolar.Models.TablaPosiciones", "TablaPosicionesLocal")
                        .WithMany()
                        .HasForeignKey("TablaPosicionesLocalId");

                    b.HasOne("TorneoSolar.Models.TablaPosiciones", "TablaPosicionesVisitante")
                        .WithMany()
                        .HasForeignKey("TablaPosicionesVisitanteId");

                    b.Navigation("EquipoLocal");

                    b.Navigation("EquipoVisitante");

                    b.Navigation("Partido");

                    b.Navigation("Resultado");

                    b.Navigation("TablaPosicionesLocal");

                    b.Navigation("TablaPosicionesVisitante");
                });

            modelBuilder.Entity("TorneoSolar.Models.Equipo", b =>
                {
                    b.HasOne("TorneoSolar.Models.Usuario", null)
                        .WithMany("Equipos")
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("TorneoSolar.Models.EstadisticasJugadore", b =>
                {
                    b.HasOne("TorneoSolar.Models.Jugadore", "Jugador")
                        .WithMany("EstadisticasJugadores")
                        .HasForeignKey("JugadorId")
                        .HasConstraintName("FK__Estadisti__Jugad__571DF1D5");

                    b.HasOne("TorneoSolar.Models.Partido", "Partido")
                        .WithMany("EstadisticasJugadores")
                        .HasForeignKey("PartidoId")
                        .HasConstraintName("FK__Estadisti__Parti__5812160E");

                    b.Navigation("Jugador");

                    b.Navigation("Partido");
                });

            modelBuilder.Entity("TorneoSolar.Models.Jugadore", b =>
                {
                    b.HasOne("TorneoSolar.Models.Equipo", "Equipo")
                        .WithMany("Jugadores")
                        .HasForeignKey("EquipoId")
                        .HasConstraintName("FK__Jugadores__Equip__4CA06362");

                    b.Navigation("Equipo");
                });

            modelBuilder.Entity("TorneoSolar.Models.Partido", b =>
                {
                    b.HasOne("TorneoSolar.Models.Equipo", "LocalEquipo")
                        .WithMany("PartidoLocalEquipos")
                        .HasForeignKey("LocalEquipoId")
                        .HasConstraintName("FK__Partidos__LocalE__4F7CD00D");

                    b.HasOne("TorneoSolar.Models.Equipo", "VisitanteEquipo")
                        .WithMany("PartidoVisitanteEquipos")
                        .HasForeignKey("VisitanteEquipoId")
                        .HasConstraintName("FK__Partidos__Visita__5070F446");

                    b.Navigation("LocalEquipo");

                    b.Navigation("VisitanteEquipo");
                });

            modelBuilder.Entity("TorneoSolar.Models.ResultadosPartido", b =>
                {
                    b.HasOne("TorneoSolar.Models.Partido", "Partido")
                        .WithOne("ResultadosPartido")
                        .HasForeignKey("TorneoSolar.Models.ResultadosPartido", "PartidoId")
                        .HasConstraintName("FK__Resultado__Parti__5441852A");

                    b.Navigation("Partido");
                });

            modelBuilder.Entity("TorneoSolar.Models.TablaPosiciones", b =>
                {
                    b.HasOne("TorneoSolar.Models.Equipo", "Equipo")
                        .WithMany()
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipo");
                });

            modelBuilder.Entity("TorneoSolar.Models.TablaPosicionesFem", b =>
                {
                    b.HasOne("TorneoSolar.Models.Equipo", "Equipo")
                        .WithMany()
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipo");
                });

            modelBuilder.Entity("TorneoSolar.Models.Equipo", b =>
                {
                    b.Navigation("Jugadores");

                    b.Navigation("PartidoLocalEquipos");

                    b.Navigation("PartidoVisitanteEquipos");
                });

            modelBuilder.Entity("TorneoSolar.Models.Jugadore", b =>
                {
                    b.Navigation("EstadisticasJugadores");
                });

            modelBuilder.Entity("TorneoSolar.Models.Partido", b =>
                {
                    b.Navigation("EstadisticasJugadores");

                    b.Navigation("ResultadosPartido");
                });

            modelBuilder.Entity("TorneoSolar.Models.Usuario", b =>
                {
                    b.Navigation("Equipos");
                });
#pragma warning restore 612, 618
        }
    }
}
