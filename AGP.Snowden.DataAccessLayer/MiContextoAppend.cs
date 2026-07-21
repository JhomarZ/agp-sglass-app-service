using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AGP.Snowden.DataAccessLayer;

public partial class MiContextoAppend : DbContext
{
    public MiContextoAppend()
    {
    }

    public MiContextoAppend(DbContextOptions<MiContextoAppend> options)
        : base(options)
    {
    }

    public virtual DbSet<PackingListStatusHistory> PackingListStatusHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=20.195.211.31;Database=DB_SNOWDEN;user id=adminsa;password=AdminS@2021!#;persist security info=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackingListStatusHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PackingListStatusHistory", "Warehouse");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Status)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
