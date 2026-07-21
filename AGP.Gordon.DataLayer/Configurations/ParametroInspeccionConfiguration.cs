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
    public class ParametroInspeccionConfiguration : IEntityTypeConfiguration<ParametrosInspeccion>
    {
        public void Configure(EntityTypeBuilder<ParametrosInspeccion> builder)
        {
            builder.ToTable("ParametrosInspeccion");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Parametro).HasMaxLength(255);
            builder.Property(e => e.Modulo).HasMaxLength(100);
            builder.Property(e => e.ValorXdefecto).HasMaxLength(255);
            builder.Property(e => e.MinimoValor).HasMaxLength(50);
            builder.Property(e => e.MaximoValor).HasMaxLength(50);
            builder.Property(e => e.Tipo).HasMaxLength(50);
            builder.Property(e => e.ParametroIngles).HasMaxLength(255);
            builder.Property(e => e.Simbolo).HasMaxLength(20);
            builder.Property(e => e.TipoEtiqueta).HasMaxLength(50);
            builder.Property(e => e.ParametroPortugues).HasMaxLength(255);

            // Conversión de bool a byte
            builder.Property(e => e.Activo).HasConversion<byte>();
            builder.Property(e => e.Peru).HasConversion<byte>();
            builder.Property(e => e.Colombia).HasConversion<byte>();
            builder.Property(e => e.Brasil).HasConversion<byte>();
            builder.Property(e => e.Calidad).HasConversion<byte>();
            builder.Property(e => e.Curvado).HasConversion<byte>();
            builder.Property(e => e.ColumnaDinamica).HasConversion<byte>();
            builder.Property(e => e.ParametroDefault).HasConversion<byte>();
            builder.Property(e => e.Requerido).HasConversion<byte>();
            builder.Property(e => e.Curvo).HasConversion<byte>();
            builder.Property(e => e.Plano).HasConversion<byte>();

            builder.HasIndex(e => e.IdCompania)
                .HasDatabaseName("IX_ParametrosInspeccion_IdCompania");

            builder.HasIndex(e => e.Modulo)
                .HasDatabaseName("IX_ParametrosInspeccion_Modulo");
        }
    }
}
