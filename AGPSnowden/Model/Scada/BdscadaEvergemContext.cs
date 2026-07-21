using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AGPSnowden.Model.Scada;

public partial class BdscadaEvergemContext : DbContext
{
    public BdscadaEvergemContext()
    {
    }

    public BdscadaEvergemContext(DbContextOptions<BdscadaEvergemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SapOrder> SapOrders { get; set; }

    public virtual DbSet<TableDefect> TableDefects { get; set; }

    public virtual DbSet<TableDefectGroupSap> TableDefectGroupSaps { get; set; }

    public virtual DbSet<TableScadaSap> TableScadaSaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=191.232.39.189\\Express,51405;Database=BDScadaEvergem;user id=sa;password=S1stemas@2020;persist security info=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SapOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SapOrder__3214EC07B5BD5B3B");

            entity.ToTable("SapOrder");

            entity.Property(e => e.Correlativo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fecha)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Hora)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Linea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Orden)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TableDefect>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Id)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ID");
        });

        modelBuilder.Entity<TableDefectGroupSap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableDefectGroupSAP");

            entity.Property(e => e.Defect)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DefectDescription)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.DefectId)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.GroupDefect)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.GroupDefectDescription)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.IdCodigo).ValueGeneratedOnAdd();
            entity.Property(e => e.WorkStationPlc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("WorkStationPLC");
        });

        modelBuilder.Entity<TableScadaSap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableScadaSap");

            entity.Property(e => e.Cicle)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Correlative)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DateModify).HasColumnType("datetime");
            entity.Property(e => e.DefectId).HasColumnName("DefectID");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IdworkCenter)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("IDWorkCenter");
            entity.Property(e => e.IpNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP_number");
            entity.Property(e => e.KeyModel)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Operation)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Order)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Plant)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Workcenter)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
