using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TorneoSolar.Models;

namespace TorneoSolar.Models;

public partial class TorneoSolarContext : DbContext
{
    public TorneoSolarContext()
    {
    }

    public TorneoSolarContext(DbContextOptions<TorneoSolarContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<EstadisticasJugadore> EstadisticasJugadores { get; set; }

    public virtual DbSet<Jugadore> Jugadores { get; set; }

    public virtual DbSet<Partido> Partidos { get; set; }

    public virtual DbSet<ResultadosPartido> ResultadosPartidos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=ALEJA\\SQLEXPRESS; database=TorneoSolar; integrated security=true; Encrypt=false;Trusted_Connection=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.EquipoId).HasName("PK__Equipos__DE8A0BFFE8D35577");

            entity.Property(e => e.EquipoId).HasColumnName("EquipoID");
            entity.Property(e => e.Ciudad).HasMaxLength(100);
            entity.Property(e => e.Logo).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<EstadisticasJugadore>(entity =>
        {
            entity.HasKey(e => e.EstadisticaId).HasName("PK__Estadist__5E78B5EC933DC896");

            entity.Property(e => e.EstadisticaId).HasColumnName("EstadisticaID");
            entity.Property(e => e.Asistencias).HasDefaultValue(0);
            entity.Property(e => e.Bloqueos).HasDefaultValue(0);
            entity.Property(e => e.JugadorId).HasColumnName("JugadorID");
            entity.Property(e => e.MinutosJugados).HasDefaultValue(0);
            entity.Property(e => e.PartidoId).HasColumnName("PartidoID");
            entity.Property(e => e.Puntos).HasDefaultValue(0);
            entity.Property(e => e.Rebotes).HasDefaultValue(0);
            entity.Property(e => e.Robos).HasDefaultValue(0);

            entity.HasOne(d => d.Jugador).WithMany(p => p.EstadisticasJugadores)
                .HasForeignKey(d => d.JugadorId)
                .HasConstraintName("FK__Estadisti__Jugad__571DF1D5");

            entity.HasOne(d => d.Partido).WithMany(p => p.EstadisticasJugadores)
                .HasForeignKey(d => d.PartidoId)
                .HasConstraintName("FK__Estadisti__Parti__5812160E");
        });

        modelBuilder.Entity<Jugadore>(entity =>
        {
            entity.HasKey(e => e.JugadorId).HasName("PK__Jugadore__4B5752421E42568D");

            entity.HasIndex(e => e.Identificacion, "UQ__Jugadore__D6F931E56DFE6D8C").IsUnique();

            entity.Property(e => e.JugadorId).HasColumnName("JugadorID");
            entity.Property(e => e.EquipoId).HasColumnName("EquipoID");
            entity.Property(e => e.Identificacion).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Peso).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Posicion).HasMaxLength(50);

            entity.HasOne(d => d.Equipo).WithMany(p => p.Jugadores)
                .HasForeignKey(d => d.EquipoId)
                .HasConstraintName("FK__Jugadores__Equip__4CA06362");
        });

        modelBuilder.Entity<Partido>(entity =>
        {
            entity.HasKey(e => e.PartidoId).HasName("PK__Partidos__DBC2E8D640879305");

            entity.Property(e => e.PartidoId).HasColumnName("PartidoID");
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.LocalEquipoId).HasColumnName("LocalEquipoID");
            entity.Property(e => e.Ubicacion).HasMaxLength(200);
            entity.Property(e => e.VisitanteEquipoId).HasColumnName("VisitanteEquipoID");

            entity.HasOne(d => d.LocalEquipo).WithMany(p => p.PartidoLocalEquipos)
                .HasForeignKey(d => d.LocalEquipoId)
                .HasConstraintName("FK__Partidos__LocalE__4F7CD00D");

            entity.HasOne(d => d.VisitanteEquipo).WithMany(p => p.PartidoVisitanteEquipos)
                .HasForeignKey(d => d.VisitanteEquipoId)
                .HasConstraintName("FK__Partidos__Visita__5070F446");
        });

        modelBuilder.Entity<ResultadosPartido>(entity =>
        {
            entity.HasKey(e => e.ResultadoId).HasName("PK__Resultad__7904DD4125ECEB63");

            entity.HasIndex(e => e.PartidoId, "UQ__Resultad__DBC2E8D77132EFE8").IsUnique();

            entity.Property(e => e.ResultadoId).HasColumnName("ResultadoID");
            entity.Property(e => e.PartidoId).HasColumnName("PartidoID");

            entity.HasOne(d => d.Partido).WithOne(p => p.ResultadosPartido)
                .HasForeignKey<ResultadosPartido>(d => d.PartidoId)
                .HasConstraintName("FK__Resultado__Parti__5441852A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<TorneoSolar.Models.Usuario> Usuario { get; set; } = default!;

public DbSet<TorneoSolar.Models.TablaPosiciones> TablaPosiciones { get; set; } = default!;

public DbSet<TorneoSolar.Models.Noticias> Noticias { get; set; }

public DbSet<TorneoSolar.Models.TablaPosicionesFem> TablaPosicionesFem { get; set; }

}
