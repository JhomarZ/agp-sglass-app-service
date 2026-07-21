using AGP.Gordon.DataAccessLayer.SAPEXPANSION;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Gordon.DataAccessLayer.Configurations
{
    public class InspeccionOpticaConfiguration: IEntityTypeConfiguration<InspeccionOptica>
    {
        public void Configure(EntityTypeBuilder<InspeccionOptica> builder)
        {
            builder.ToTable("InspeccionOptica");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Parametro)
                .HasMaxLength(255);

            builder.Property(e => e.Observacion)
                .HasMaxLength(500);

            builder.Property(e => e.UsuarioCrea)
                .HasMaxLength(50);

            builder.Property(e => e.UsuarioEdita)
                .HasMaxLength(50);


            builder.HasOne(e => e.ParametroInspeccion)
                .WithMany(p => p.InspeccionesOpticas)
                .HasForeignKey(e => e.ParametroInspeccionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.CertificadoId)
                .HasDatabaseName("IX_InspeccionOptica_CertificadoId");

            builder.HasIndex(e => e.ParametroInspeccionId)
                .HasDatabaseName("IX_InspeccionOptica_ParametroInspeccionId");
        }
    }

    public class InspeccionDimensionalConfiguration : IEntityTypeConfiguration<CertificadoIfdimension>
    {
        public void Configure(EntityTypeBuilder<CertificadoIfdimension> builder)
        {
            builder.ToTable("CertificadoIFDimension");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Parametro)
                .HasMaxLength(255);

            builder.Property(e => e.Observacion)
                .HasMaxLength(500);

            builder.Property(e => e.UsuarioCrea)
                .HasMaxLength(50);

            builder.Property(e => e.UsuarioEdita)
                .HasMaxLength(50);


            builder.HasOne(e => e.ParametroInspeccion)
                .WithMany(p => p.InspeccionesDimensional)
                .HasForeignKey(e => e.ParametroInspeccionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.CertificadoId)
                .HasDatabaseName("IX_CertificadoIFDimension_CertificadoId");

            builder.HasIndex(e => e.ParametroInspeccionId)
                .HasDatabaseName("IX_CertificadoIFDimension_ParametroInspeccionId");
        }
    }

    public class InspeccionAparienciaConfiguration : IEntityTypeConfiguration<CertificadoIfapariencias>
    {
        public void Configure(EntityTypeBuilder<CertificadoIfapariencias> builder)
        {
            builder.ToTable("CertificadoIFApariencia");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Parametro)
                .HasMaxLength(255);

            builder.Property(e => e.Observacion)
                .HasMaxLength(500);

            builder.Property(e => e.UsuarioCrea)
                .HasMaxLength(50);

            builder.Property(e => e.UsuarioEdita)
                .HasMaxLength(50);


            builder.HasOne(e => e.ParametroInspeccion)
                .WithMany(p => p.InspeccionesApariencia)
                .HasForeignKey(e => e.ParametroInspeccionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.CertificadoId)
                .HasDatabaseName("IX_CertificadoIfapariencias_CertificadoId");

            builder.HasIndex(e => e.ParametroInspeccionId)
                .HasDatabaseName("IX_CertificadoIfapariencias_ParametroInspeccionId");
        }
    }
}
