using System;
using System.Collections.Generic;
using AGP.Gordon.DataAccessLayer.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class SapexpansionContext : DbContext
{
    public SapexpansionContext()
    {
    }

    public SapexpansionContext(DbContextOptions<SapexpansionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BallisticTest> BallisticTests { get; set; }

    public virtual DbSet<CertificadoIf> CertificadoIfs { get; set; }

    public virtual DbSet<CertificadoIfapariencias> CertificadoIfapariencia { get; set; }

    public virtual DbSet<CertificadoIfdimension> CertificadoIfdimensions { get; set; }

    public virtual DbSet<ClasificadorPadre> ClasificadorPadres { get; set; }

    public virtual DbSet<Clasificadore> Clasificadores { get; set; }

    public virtual DbSet<Curvado> Curvados { get; set; }

    public virtual DbSet<Defectos> Defectos { get; set; }

    public virtual DbSet<HerramentalProceso> HerramentalProcesos { get; set; }

    public virtual DbSet<InspeccionOptica> InspeccionOpticas { get; set; }

    public virtual DbSet<ParametrosInspeccion> ParametrosInspeccions { get; set; }

    public virtual DbSet<ParametrosInspeccionTmp> ParametrosInspeccionTmps { get; set; }

    public virtual DbSet<PiezaBloqueadum> PiezaBloqueada { get; set; }

    public virtual DbSet<PiezaConcesion> PiezaConcesions { get; set; }

    public virtual DbSet<PiezaSap> PiezaSaps { get; set; }

    public virtual DbSet<Recetum> Receta { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<TipoPieza> TipoPiezas { get; set; }

    public virtual DbSet<UsuarioCompanium> UsuarioCompania { get; set; }

    public virtual DbSet<UsuariosAgp> UsuariosAgps { get; set; }

    public virtual DbSet<ViewParametrosInspeccion> ViewParametrosInspeccions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=sqlgordon.database.windows.net;Database=SQLGORDON1;user id=GordonSADMIN;password=#SglassMudaGORDON25;persist security info=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<BallisticTest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BallisticTest");

            entity.Property(e => e.AmbientTemperature)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AmmunitionWheight)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.AverageThickness)
                .HasMaxLength(250)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Conditioning)
                .HasMaxLength(250)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.EfectoA)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EfectoB)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EfectoC)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EfectoD)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.EfectoE)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Formulas)
                .HasMaxLength(40)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.GlassTransparency)
                .HasMaxLength(40)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Gun)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ImagenBs).HasColumnName("ImagenBS");
            entity.Property(e => e.ImagenFs).HasColumnName("ImagenFS");
            entity.Property(e => e.ImagenProf).HasColumnName("ImagenPROF");
            entity.Property(e => e.ImagenWai).HasColumnName("ImagenWAI");
            entity.Property(e => e.ImagenWbi).HasColumnName("ImagenWBI");
            entity.Property(e => e.ImagenWi).HasColumnName("ImagenWI");
            entity.Property(e => e.PreparedFor)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Probeta)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProjectileCaliber)
                .HasMaxLength(40)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RelativeHumidity)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.RequiredBulletVelocity)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.ShootingPattern)
                .HasMaxLength(200)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TestDistance)
                .HasMaxLength(20)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.TestSpecification)
                .HasMaxLength(40)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.VelocidadA).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.VelocidadB).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.VelocidadC).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.VelocidadD).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.VelocidadE).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<CertificadoIf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CertificadoIf_ID");

            entity.ToTable("CertificadoIF");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Autoriza)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCrea)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.FechaTermino).HasColumnType("datetime");
            entity.Property(e => e.ImagenDblZonaA)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImagenDblZonaB)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImagenDistorcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ImagenOp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ImagenOP");
            entity.Property(e => e.NroColumnas).HasDefaultValueSql("((12))");
            entity.Property(e => e.Observacion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrdProceso)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Revisa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TerminadoApariencia).HasDefaultValueSql("((0))");
            entity.Property(e => e.TerminadoDimensional).HasDefaultValueSql("((0))");
            entity.Property(e => e.TerminadoElectrico).HasDefaultValueSql("((0))");
            entity.Property(e => e.TerminadoOptico).HasDefaultValueSql("((0))");
            entity.Property(e => e.Termino).HasDefaultValueSql("((0))");
            entity.Property(e => e.TipoPieza)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Zfer)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CertificadoIfapariencias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CertificadoIfApariencia_ID");

            entity.ToTable("CertificadoIFApariencia");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCrea).HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.MaximoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MinimoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Parametro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TipoDato)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Valor)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CertificadoIfdimension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CertificadoIFDimension_ID");

            entity.ToTable("CertificadoIFDimension");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.ColumnaDinamica).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaCrea).HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.FueraRango).HasDefaultValueSql("((0))");
            entity.Property(e => e.MaximoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MinimoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Modulo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.NoAplica).HasDefaultValueSql("((0))");
            entity.Property(e => e.NroColumnas).HasDefaultValueSql("((1))");
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Origen).HasDefaultValueSql("((1))");
            entity.Property(e => e.Parametro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Val1)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val10)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val11)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val12)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val13)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val14)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val15)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val16)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val17)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val18)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val19)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val2)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val20)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val21)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val22)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val23)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val24)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val25)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val3)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val4)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val5)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val6)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val7)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val8)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val9)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ClasificadorPadre>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ClasificadorPadre");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Clasificadore>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IdCompania).HasDefaultValueSql("((1001))");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Curvado>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Curvado");

            entity.Property(e => e.Autoriza)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaCrea).HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.FechaTermino).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ImagenOp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ImagenOP");
            entity.Property(e => e.Observacion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OrdProceso)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Revisa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Zfer)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Defectos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Defectos__3214EC074E88ABD4");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Area)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Defecto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Defecto");
            entity.Property(e => e.Grupo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreIngles)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HerramentalProceso>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HerramentalProceso");

            entity.Property(e => e.CodHerramienta)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Herramienta)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.OrdProceso)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InspeccionOptica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InspeccionOptica_ID");

            entity.ToTable("InspeccionOptica");

            entity.Property(e => e.FechaCrea).HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Parametro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ParametrosInspeccion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ParametrosInspeccion");

            entity.Property(e => e.Curvo).HasDefaultValueSql("((0))");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MaximoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MinimoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Modulo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Parametro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParametroIngles)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParametroPortugues)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Plano).HasDefaultValueSql("((0))");
            entity.Property(e => e.Requerido).HasDefaultValueSql("((0))");
            entity.Property(e => e.Simbolo)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoEtiqueta)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ValorXdefecto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ValorXDefecto");
        });

        modelBuilder.Entity<ParametrosInspeccionTmp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ParametrosInspeccionTmp");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MaximoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MinimoValor)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Modulo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Parametro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParametroIngles)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ValorXdefecto)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ValorXDefecto");
        });

        modelBuilder.Entity<PiezaBloqueadum>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Activo)
                .HasDefaultValueSql("((0))")
                .HasColumnName("activo");
            entity.Property(e => e.ActualizadoEl)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("actualizado_el");
            entity.Property(e => e.ActualizadoPor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("actualizado_por");
            entity.Property(e => e.BloqueoId).HasColumnName("Bloqueo_id");
            entity.Property(e => e.CompanyId)
                .HasDefaultValueSql("((1001))")
                .HasColumnName("company_id");
            entity.Property(e => e.CreadoEl)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creado_el");
            entity.Property(e => e.CreadoPor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("creado_por");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.OrdenProduccion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Orden_Produccion");
            entity.Property(e => e.Zfer)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("zfer");
        });

        modelBuilder.Entity<PiezaConcesion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PiezaConcesion");

            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Archivo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Autorizado).HasDefaultValueSql("((0))");
            entity.Property(e => e.AutorizadoFecha).HasDefaultValueSql("((0))");
            entity.Property(e => e.AutorizadoPor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Color)
                .HasMaxLength(7)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Defecto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaCrea).HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Justificacion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Mercado)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.OrdProceso)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PositionX).HasColumnName("positionX");
            entity.Property(e => e.PositionY).HasColumnName("positionY");
            entity.Property(e => e.Riesgo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Supervisor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Tamanio).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.TipoDescripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Valor)
                .HasDefaultValueSql("((0))")
                .HasColumnType("decimal(8, 2)");
        });

        modelBuilder.Entity<PiezaSap>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PiezaSAP", tb => tb.HasTrigger("TR_CODIGO_IMAGEN_FT"));

            entity.Property(e => e.Altura)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Area)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BombaA)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Bomba_A");
            entity.Property(e => e.BombaB)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Bomba_B");
            entity.Property(e => e.BombaC)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Bomba_C");
            entity.Property(e => e.BombaD)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Bomba_D");
            entity.Property(e => e.Breit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BREIT");
            entity.Property(e => e.Cliente)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CodigoImagenStandar)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodigoImagenTecnica)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Documento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Espesor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaCrea)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.Formula)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ImagenFt)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ImagenFT");
            entity.Property(e => e.Laeng)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LAENG");
            entity.Property(e => e.Logo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LoteLogistico)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Matnr01)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MATNR_01");
            entity.Property(e => e.Matnr02)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MATNR_02");
            entity.Property(e => e.MedCurvatura)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Modulo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('Tablet')");
            entity.Property(e => e.Nivel)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NroPedidoUro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NroPedidoURO");
            entity.Property(e => e.Ocn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OCN");
            entity.Property(e => e.OrdProceso)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PartNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Perimetro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Pieza)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Resistencia)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ResponsableFt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ResponsableFT");
            entity.Property(e => e.Ruta)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RutaImagen).IsUnicode(false);
            entity.Property(e => e.TerminadoApariencia).HasDefaultValueSql("((0))");
            entity.Property(e => e.TerminadoDimensional).HasDefaultValueSql("((0))");
            entity.Property(e => e.TerminadoOptico).HasDefaultValueSql("((0))");
            entity.Property(e => e.TipoPedido)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TolBombaA)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Tol_Bomba_A");
            entity.Property(e => e.TolBombaB)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Tol_Bomba_B");
            entity.Property(e => e.TolBombaC)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Tol_Bomba_C");
            entity.Property(e => e.TolBombaD)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("Tol_Bomba_D");
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Vehiculo)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Vidrio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Zfer)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Recetum>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Cabina)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaCrea).HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.Grados)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Horno)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Observacion)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Operacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tareas__3214EC075441852A");

            entity.Property(e => e.FechaCrea).HasColumnType("datetime");
            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.Imagen)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.OrdProceso)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Responsable)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCrea)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoPieza>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoPiez__3214EC072645B050");

            entity.ToTable("TipoPieza");

            entity.Property(e => e.Abreviatiura)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.NombreIngles)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsuarioCompanium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UsuarioC__3214EC0732AB8735");

            entity.Property(e => e.CompaniaNombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompaniaOrigen).HasDefaultValueSql("((0))");
            entity.Property(e => e.IdCompania)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsuariosAgp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UsuariosAgp");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Area)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewParametrosInspeccion>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Parametros_Inspeccion");

            entity.Property(e => e.FechaEdita).HasColumnType("datetime");
            entity.Property(e => e.Modulo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Parametro)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioEdita)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Val1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Val10)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val11)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val12)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val13)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val14)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val15)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val16)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val17)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val18)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val19)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val2)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val20)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val3)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val4)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val5)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val6)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val7)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val8)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Val9)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.ApplyConfiguration(new ParametroInspeccionConfiguration());
        modelBuilder.ApplyConfiguration(new InspeccionOpticaConfiguration());

        modelBuilder.ApplyConfiguration(new InspeccionDimensionalConfiguration());
        modelBuilder.ApplyConfiguration(new InspeccionAparienciaConfiguration());



    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
