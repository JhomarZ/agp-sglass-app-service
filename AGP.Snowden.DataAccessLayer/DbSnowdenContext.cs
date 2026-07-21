using System;
using System.Collections.Generic;
using AGP.Security.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.RRHH;
using AGP.Snowden.DataAccessLayer.SAP;
using Microsoft.EntityFrameworkCore;

namespace AGP.Snowden.DataAccessLayer;

public partial class DbSnowdenContext : DbContext
{
    public DbSnowdenContext()
    {
    }

    public DbSnowdenContext(DbContextOptions<DbSnowdenContext> options)
        : base(options)
    {
    }


    public virtual DbSet<TrackingImputadosExtension> TrackingImputadosExtensions { get; set; }

    public virtual DbSet<PP_TRACKNG_IMPUT> TrackingImputadosSap { get; set; }
    

    public virtual DbSet<SpectroRequest> SpectroRequests { get; set; }

    public virtual DbSet<MeasurementType> MeasurementTypes { get; set; }

    public virtual DbSet<ValidationPlan> ValidationPlans { get; set; }

    public virtual DbSet<MaterialCategory> MaterialCategories { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<MaterialTemplate> MaterialTemplates { get; set; }

    public virtual DbSet<InspectionPlan> InspectionPlans { get; set; }

    public virtual DbSet<CharacteristicInspectionPlan> CharacteristicInspectionPlans { get; set; }

    public virtual DbSet<CharacteristicInput> CharacteristicInputs { get; set; }

    public virtual DbSet<Material> Materials { get; set; }
    

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Audit> Audits { get; set; }

    public virtual DbSet<AuditChecksList> AuditChecksLists { get; set; }

    public virtual DbSet<AuditSubType> AuditSubTypes { get; set; }

    public virtual DbSet<AuditSubType1> AuditSubTypes1 { get; set; }

    public virtual DbSet<AuditType> AuditTypes { get; set; }

    public virtual DbSet<Centro> Centros { get; set; }

    public virtual DbSet<CheckListSegment> CheckListSegments { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Defect> Defects { get; set; }

    public virtual DbSet<DefectsType> DefectsTypes { get; set; }

    public virtual DbSet<EssayNorma> EssayNormas { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<MotivesToStop> MotivesToStops { get; set; }

    public virtual DbSet<MotivesTypeToStop> MotivesTypeToStops { get; set; }

    public virtual DbSet<Norma> Normas { get; set; }

    public virtual DbSet<PressTissueControl> PressTissueControls { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<ProcessDefect> ProcessDefects { get; set; }

    public virtual DbSet<ProcessDefectType> ProcessDefectTypes { get; set; }

    public virtual DbSet<ProcessMotivesToStop> ProcessMotivesToStops { get; set; }

    public virtual DbSet<ProcessOrigin> ProcessOrigins { get; set; }

    public virtual DbSet<ProcessProduct> ProcessProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductionRecord> ProductionRecords { get; set; }

    public virtual DbSet<ProductionRecordsDetail> ProductionRecordsDetails { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileModule> ProfileModules { get; set; }

    public virtual DbSet<Programa> Programas { get; set; }

    public virtual DbSet<ProgramaCabecera> ProgramaCabeceras { get; set; }

    public virtual DbSet<ProjectManager> ProjectManagers { get; set; }

    public virtual DbSet<Rdproject> Rdprojects { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ReportPbi> ReportPbis { get; set; }

    public virtual DbSet<Responsable> Responsables { get; set; }

    public virtual DbSet<Technical> Technicals { get; set; }

    public virtual DbSet<Technology> Technologies { get; set; }

    public virtual DbSet<TecnosenFile> TecnosenFiles { get; set; }

    public virtual DbSet<TecnosenProgram> TecnosenPrograms { get; set; }

    public virtual DbSet<TecnosenRecord> TecnosenRecords { get; set; }

    public virtual DbSet<TestParameterInspection> TestParameterInspections { get; set; }

    public virtual DbSet<TestParameterInspectionClassifier> TestParameterInspectionClassifiers { get; set; }

    public virtual DbSet<TestRequest> TestRequests { get; set; }

    public virtual DbSet<TestRequestClassifier> TestRequestClassifiers { get; set; }

    public virtual DbSet<TestRequestFile> TestRequestFiles { get; set; }

    public virtual DbSet<TestRequestMeasurement> TestRequestMeasurements { get; set; }

    public virtual DbSet<TestRequestParameterInspection> TestRequestParameterInspections { get; set; }

    public virtual DbSet<TestRequestParameterInspectionTmp> TestRequestParameterInspectionTmps { get; set; }

    public virtual DbSet<TestRequestStatus> TestRequestStatuses { get; set; }

    public virtual DbSet<TestRequestStatusHistory> TestRequestStatusHistories { get; set; }

    public virtual DbSet<Testdatum> Testdata { get; set; }

    public virtual DbSet<TypeEssay> TypeEssays { get; set; }

    public virtual DbSet<UserCentro> UserCentros { get; set; }

    public virtual DbSet<UserModule> UserModules { get; set; }

    public virtual DbSet<UserSystem> UserSystems { get; set; }

    public virtual DbSet<VbiproductControlTissue> VbiproductControlTissues { get; set; }

    public virtual DbSet<VprocessDefect> VprocessDefects { get; set; }

    public virtual DbSet<VprocessProduct> VprocessProducts { get; set; }

    public virtual DbSet<VprofileModule> VprofileModules { get; set; }

    public virtual DbSet<VwParameterInspection> VwParameterInspections { get; set; }

    public virtual DbSet<VwTestRequestMeasurement> VwTestRequestMeasurements { get; set; }

    public virtual DbSet<VwTestRequestMeasurementsUnpivot> VwTestRequestMeasurementsUnpivots { get; set; }

    public virtual DbSet<PackingList> PackingLists { get; set; }

    public virtual DbSet<PackingListItem> PackingListItems { get; set; }

    public virtual DbSet<PackingListStatus> PackingListStatuses { get; set; }

    public virtual DbSet<ShippingStatus> ShippingStatuses { get; set; }

    public virtual DbSet<VwimputadosSap> VwimputadoSap { get; set; }

    public virtual DbSet<ImputadoStatusHistory> ImputadoStatusHistories { get; set; }

    public virtual DbSet<PackingListStatusHistory> PackingListStatusHistories { get; set; }

    public virtual DbSet<Personal> Personal{ get; set; }

    public virtual DbSet<LoanMoneyRequest> LendMoneyRequest { get; set; }

    public virtual DbSet<LoanMoneyRequestStatusHistory> LoanMoneyRequestStatusHistory { get; set; }

    public virtual DbSet<LoanRequestFile> LoanRequestFiles { get; set; }

    public virtual DbSet<ReasonLendRequest> ReasonLendRequest { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=20.195.211.31;Database=DB_SNOWDEN;user id=adminsa;password=AdminS@2021!#;persist security info=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Area");

            entity.ToTable("Area");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Company).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NameEng).HasMaxLength(100);
        });

        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__audits__3214EC0707F7BE39");

            entity.ToTable("audits", "Production");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.GeneralComment)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.HasNc).HasColumnName("HasNC");
            entity.Property(e => e.Observation)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ProductionOrder)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Resonsable)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Shift)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Validation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ValidationQuality)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ValidationQualityText)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ValidationText)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Zona)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuditChecksList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditChe__3214EC07744CC6DF");

            entity.ToTable("AuditChecksList");

            entity.Property(e => e.Attachment)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CheckName)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Created_by");
            entity.Property(e => e.ImageA)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.ImageB)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.InputType)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Max).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.Min).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.Observation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ObservationQuality)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ObservationSupervisor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Options)
                .HasMaxLength(700)
                .IsUnicode(false);
            entity.Property(e => e.Responsable)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tag)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Updated_by");
            entity.Property(e => e.Value)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuditSubType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__audit_su__3213E83F67D126D4");

            entity.ToTable("audit_sub_types", "Production");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.AuditTypeId).HasColumnName("audit_type_id");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("centro");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("compania");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.HidePo).HasColumnName("hidePO");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OperationalSpecs)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("operational_specs");
            entity.Property(e => e.ProcessId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("process_id");
            entity.Property(e => e.Structure)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("structure");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("updated_by");
            entity.Property(e => e.Version)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("version");
            entity.Property(e => e.Zona)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("zona");
        });

        modelBuilder.Entity<AuditSubType1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditSub__3214EC072F42B158");

            entity.ToTable("AuditSubTypes", "Production");

            entity.Property(e => e.AuditTypeId).HasColumnName("AuditType_Id");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("Company_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Created_by");
            entity.Property(e => e.HidePo).HasColumnName("HidePO");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.OperationalSpecs)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Operational_specs");
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
            entity.Property(e => e.Structure)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Updated_by");
            entity.Property(e => e.Version)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Zona)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuditType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditTyp__3214EC0745E1DA74");

            entity.ToTable("AuditTypes", "Production");

            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("Company_Id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Created_by");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Updated_by");
            entity.Property(e => e.Zona)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Centro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Centro");

            entity.ToTable("Centro");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CheckListSegment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CheckLis__3214EC071B5F921C");

            entity.ToTable("CheckListSegment", "Production");

            entity.Property(e => e.Attachment)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Created_by");
            entity.Property(e => e.InputType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Input_type");
            entity.Property(e => e.Max).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.Min).HasColumnType("decimal(9, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Options)
                .HasMaxLength(800)
                .IsUnicode(false);
            entity.Property(e => e.Tag)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Updated_at");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("Updated_by");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Company");

            entity.ToTable("Company");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Defect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Defects__3214EC075A495CC9");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DefectsType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DefectsT__3214EC07B26A3BC0");

            entity.ToTable("DefectsType");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EssayNorma>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EssayNor__3214EC073F972CA4");

            entity.ToTable("EssayNorma");

            entity.Property(e => e.EssayId).HasColumnName("Essay_Id");
            entity.Property(e => e.NormaId).HasColumnName("Norma_Id");
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");

            entity.ToTable("__MigrationHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ContextKey).HasMaxLength(300);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Module");

            entity.ToTable("Module");

            entity.Property(e => e.Centro).HasMaxLength(10);
            entity.Property(e => e.Compania).HasMaxLength(10);
            entity.Property(e => e.Controller).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.EmbedPbi).HasColumnName("EmbedPBI");
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<MotivesToStop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MotivesT__3214EC07DFBE6B43");

            entity.ToTable("MotivesToStop");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MotivesTypeToStopId).HasColumnName("MotivesTypeToStop_Id");
            entity.Property(e => e.MotivesTypeToStopId1).HasColumnName("MotivesTypeToStop_Id1");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TagAvo)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasDefaultValueSql("('SIN TAG')")
                .HasColumnName("TAG_AVO");
        });

        modelBuilder.Entity<MotivesTypeToStop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MotivesT__3214EC07916BB901");

            entity.ToTable("MotivesTypeToStop");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.MotivesTypeToStopId).HasColumnName("MotivesTypeToStop_Id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Norma>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Normas__3214EC077F93DE2E");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Link)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PressTissueControl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PressTis__3213E83FFB6C9200");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Cause)
                .HasMaxLength(200)
                .HasColumnName("cause");
            entity.Property(e => e.Center)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("center");
            entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(80)
                .HasColumnName("createdBy");
            entity.Property(e => e.Observation)
                .HasMaxLength(250)
                .HasColumnName("observation");
            entity.Property(e => e.PartsPressed).HasColumnName("partsPressed");
            entity.Property(e => e.Photo)
                .HasMaxLength(50)
                .HasColumnName("photo");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .HasColumnName("position");
            entity.Property(e => e.PressType)
                .HasMaxLength(10)
                .HasColumnName("pressType");
            entity.Property(e => e.ProductId).HasColumnName("product_Id");
            entity.Property(e => e.Shift)
                .HasMaxLength(1)
                .HasColumnName("shift");
            entity.Property(e => e.Tissue)
                .HasMaxLength(100)
                .HasColumnName("tissue");
            entity.Property(e => e.UpdatedAt).HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(80)
                .HasColumnName("updatedBy");
            entity.Property(e => e.WhenDate)
                .HasMaxLength(255)
                .HasColumnName("whenDate");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Process__3214EC07529966AE");

            entity.ToTable("Process");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tag)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<ProcessDefect>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.DefectId).HasColumnName("Defect_Id");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
        });

        modelBuilder.Entity<ProcessDefectType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ProcessDefectType");

            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.DefectTypeId).HasColumnName("DefectType_Id");
            entity.Property(e => e.DefectTypeId1).HasColumnName("DefectType_Id1");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
        });

        modelBuilder.Entity<ProcessMotivesToStop>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MotivesToStopId).HasColumnName("MotivesToStop_Id");
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
        });

        modelBuilder.Entity<ProcessOrigin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ProcessOrigin");

            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
            entity.Property(e => e.ProcessOriginId).HasColumnName("Process_Origin_Id");
        });

        modelBuilder.Entity<ProcessProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ProcessProduct");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07F3035736");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductionRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC074D48CF50");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Campaign)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateRecord)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ProductionOrder)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShiftRecord)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.TimeEnd)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TimeInit)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductionRecordsDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC07FA1300C0");

            entity.ToTable("ProductionRecordsDetail");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MotiveStopedId).HasColumnName("MotiveStoped_Id");
            entity.Property(e => e.ProcessOriginId).HasColumnName("Process_Origin_Id");
            entity.Property(e => e.ProductionRecordId).HasColumnName("ProductionRecord_Id");
            entity.Property(e => e.TypeRecord)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Profile");

            entity.ToTable("Profile");

            entity.Property(e => e.Description).HasMaxLength(80);
            entity.Property(e => e.Name).HasMaxLength(80);
        });

        modelBuilder.Entity<ProfileModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ProfileModule");

            entity.ToTable("ProfileModule");
        });

        modelBuilder.Entity<Programa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Programa__3214EC07DC7DE267");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProgramaCabecera>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Programa__3214EC071BDE9A1C");

            entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
            entity.Property(e => e.CabeceraNombre)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Caracteristica)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Maximo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Minimo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Nominal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProgramaId).HasColumnName("Programa_Id");
        });

        modelBuilder.Entity<ProjectManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProjectM__3214EC07E772F29D");

            entity.ToTable("ProjectManager");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rdproject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RDProjec__3214EC07C28D1661");

            entity.ToTable("RDProjects");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InternalOrder)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observation)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Process)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProjectLeader)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RelevantFile)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SubTechnology)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.RefreshToken");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ReportPbi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ReportPBI");

            entity.ToTable("ReportPBI");

            entity.Property(e => e.AplicationId).HasMaxLength(60);
            entity.Property(e => e.Centro).HasMaxLength(10);
            entity.Property(e => e.Compania).HasMaxLength(10);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(80);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.ReportId).HasMaxLength(60);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(80);
            entity.Property(e => e.WorkspaceId).HasMaxLength(60);
        });

        modelBuilder.Entity<Responsable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Responsa__3213E83F78DA35D8");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Center)
                .HasMaxLength(255)
                .HasColumnName("center");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.Module)
                .HasMaxLength(255)
                .HasColumnName("module");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Technical>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Technology>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.NameTechnology)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TecnosenFile>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AudFecCrea).HasColumnType("datetime");
            entity.Property(e => e.AudFecModi).HasColumnType("datetime");
            entity.Property(e => e.AudIp)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AudTerminal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AudUsuCrea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AudUsuModi)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BatchDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BatchName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Channels)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Probes)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Program)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShapeFile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TecnosenName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TecnosenProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tecnosen__3214EC07387767C3");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Program)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TecnosenRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tecnosen__3214EC0760B7732F");

            entity.ToTable("TecnosenRecord");

            entity.Property(e => e.AudEstado)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.AudFecCrea)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.AudFecMofi)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.AudOpe)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AudTerminal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AudUsuCrea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AudUsuModi)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BatchDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BatchName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Channels)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CodigoBarra)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Comments)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.DateFile).HasColumnType("date");
            entity.Property(e => e.Dato1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dato2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Formato)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HourRecord)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Inspector)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.MachinId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Machin_Id");
            entity.Property(e => e.NombreArchivo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PartName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PartNumber)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Probes)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Program)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProgramaId).HasColumnName("Programa_Id");
            entity.Property(e => e.Project)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ShapeFile)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SuplierCode)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Suplier_Code");
            entity.Property(e => e.TecnosenFileId).HasColumnName("TecnosenFile_Id");
            entity.Property(e => e.TecnosenName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToolingId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tooling_Id");
            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Tracking_Number");
            entity.Property(e => e.Val1)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val1");
            entity.Property(e => e.Val10)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val10");
            entity.Property(e => e.Val100)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val100");
            entity.Property(e => e.Val101)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val101");
            entity.Property(e => e.Val102)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val102");
            entity.Property(e => e.Val103)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val103");
            entity.Property(e => e.Val104)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val104");
            entity.Property(e => e.Val105)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val105");
            entity.Property(e => e.Val106)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val106");
            entity.Property(e => e.Val107)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val107");
            entity.Property(e => e.Val108)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val108");
            entity.Property(e => e.Val109)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val109");
            entity.Property(e => e.Val11)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val11");
            entity.Property(e => e.Val110)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val110");
            entity.Property(e => e.Val111)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val111");
            entity.Property(e => e.Val112)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val112");
            entity.Property(e => e.Val113)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val113");
            entity.Property(e => e.Val114)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val114");
            entity.Property(e => e.Val115)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val115");
            entity.Property(e => e.Val116)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val116");
            entity.Property(e => e.Val117)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val117");
            entity.Property(e => e.Val118)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val118");
            entity.Property(e => e.Val119)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val119");
            entity.Property(e => e.Val12)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val12");
            entity.Property(e => e.Val120)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val120");
            entity.Property(e => e.Val121)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val121");
            entity.Property(e => e.Val122)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val122");
            entity.Property(e => e.Val123)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val123");
            entity.Property(e => e.Val124)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val124");
            entity.Property(e => e.Val125)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val125");
            entity.Property(e => e.Val126)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val126");
            entity.Property(e => e.Val127)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val127");
            entity.Property(e => e.Val128)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val128");
            entity.Property(e => e.Val129)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val129");
            entity.Property(e => e.Val13)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val13");
            entity.Property(e => e.Val130)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val130");
            entity.Property(e => e.Val131)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val131");
            entity.Property(e => e.Val132)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val132");
            entity.Property(e => e.Val133)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val133");
            entity.Property(e => e.Val134)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val134");
            entity.Property(e => e.Val135)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val135");
            entity.Property(e => e.Val136)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val136");
            entity.Property(e => e.Val137)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val137");
            entity.Property(e => e.Val138)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val138");
            entity.Property(e => e.Val139)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val139");
            entity.Property(e => e.Val14)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val14");
            entity.Property(e => e.Val140)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val140");
            entity.Property(e => e.Val141)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val141");
            entity.Property(e => e.Val142)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val142");
            entity.Property(e => e.Val143)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val143");
            entity.Property(e => e.Val144)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val144");
            entity.Property(e => e.Val145)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val145");
            entity.Property(e => e.Val146)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val146");
            entity.Property(e => e.Val147)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val147");
            entity.Property(e => e.Val148)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val148");
            entity.Property(e => e.Val149)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val149");
            entity.Property(e => e.Val15)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val15");
            entity.Property(e => e.Val150)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val150");
            entity.Property(e => e.Val151)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val151");
            entity.Property(e => e.Val152)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val152");
            entity.Property(e => e.Val153)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val153");
            entity.Property(e => e.Val154)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val154");
            entity.Property(e => e.Val155)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val155");
            entity.Property(e => e.Val156)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val156");
            entity.Property(e => e.Val157)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val157");
            entity.Property(e => e.Val158)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val158");
            entity.Property(e => e.Val159)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val159");
            entity.Property(e => e.Val16)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val16");
            entity.Property(e => e.Val160)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val160");
            entity.Property(e => e.Val161)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val161");
            entity.Property(e => e.Val162)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val162");
            entity.Property(e => e.Val163)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val163");
            entity.Property(e => e.Val164)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val164");
            entity.Property(e => e.Val165)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val165");
            entity.Property(e => e.Val166)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val166");
            entity.Property(e => e.Val167)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val167");
            entity.Property(e => e.Val168)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val168");
            entity.Property(e => e.Val169)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val169");
            entity.Property(e => e.Val17)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val17");
            entity.Property(e => e.Val170)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val170");
            entity.Property(e => e.Val171)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val171");
            entity.Property(e => e.Val172)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val172");
            entity.Property(e => e.Val173)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val173");
            entity.Property(e => e.Val174)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val174");
            entity.Property(e => e.Val175)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val175");
            entity.Property(e => e.Val176)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val176");
            entity.Property(e => e.Val177)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val177");
            entity.Property(e => e.Val178)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val178");
            entity.Property(e => e.Val179)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val179");
            entity.Property(e => e.Val18)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val18");
            entity.Property(e => e.Val180)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val180");
            entity.Property(e => e.Val181)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val181");
            entity.Property(e => e.Val182)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val182");
            entity.Property(e => e.Val183)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val183");
            entity.Property(e => e.Val184)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val184");
            entity.Property(e => e.Val185)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val185");
            entity.Property(e => e.Val186)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val186");
            entity.Property(e => e.Val187)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val187");
            entity.Property(e => e.Val188)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val188");
            entity.Property(e => e.Val189)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val189");
            entity.Property(e => e.Val19)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val19");
            entity.Property(e => e.Val190)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val190");
            entity.Property(e => e.Val191)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val191");
            entity.Property(e => e.Val192)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val192");
            entity.Property(e => e.Val193)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val193");
            entity.Property(e => e.Val194)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val194");
            entity.Property(e => e.Val195)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val195");
            entity.Property(e => e.Val196)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val196");
            entity.Property(e => e.Val197)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val197");
            entity.Property(e => e.Val198)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val198");
            entity.Property(e => e.Val199)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val199");
            entity.Property(e => e.Val2)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val2");
            entity.Property(e => e.Val20)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val20");
            entity.Property(e => e.Val200)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val200");
            entity.Property(e => e.Val21)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val21");
            entity.Property(e => e.Val22)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val22");
            entity.Property(e => e.Val23)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val23");
            entity.Property(e => e.Val24)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val24");
            entity.Property(e => e.Val25)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val25");
            entity.Property(e => e.Val26)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val26");
            entity.Property(e => e.Val27)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val27");
            entity.Property(e => e.Val28)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val28");
            entity.Property(e => e.Val29)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val29");
            entity.Property(e => e.Val3)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val3");
            entity.Property(e => e.Val30)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val30");
            entity.Property(e => e.Val31)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val31");
            entity.Property(e => e.Val32)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val32");
            entity.Property(e => e.Val33)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val33");
            entity.Property(e => e.Val34)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val34");
            entity.Property(e => e.Val35)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val35");
            entity.Property(e => e.Val36)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val36");
            entity.Property(e => e.Val37)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val37");
            entity.Property(e => e.Val38)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val38");
            entity.Property(e => e.Val39)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val39");
            entity.Property(e => e.Val4)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val4");
            entity.Property(e => e.Val40)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val40");
            entity.Property(e => e.Val41)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val41");
            entity.Property(e => e.Val42)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val42");
            entity.Property(e => e.Val43)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val43");
            entity.Property(e => e.Val44)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val44");
            entity.Property(e => e.Val45)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val45");
            entity.Property(e => e.Val46)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val46");
            entity.Property(e => e.Val47)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val47");
            entity.Property(e => e.Val48)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val48");
            entity.Property(e => e.Val49)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val49");
            entity.Property(e => e.Val5)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val5");
            entity.Property(e => e.Val50)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val50");
            entity.Property(e => e.Val51)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val51");
            entity.Property(e => e.Val52)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val52");
            entity.Property(e => e.Val53)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val53");
            entity.Property(e => e.Val54)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val54");
            entity.Property(e => e.Val55)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val55");
            entity.Property(e => e.Val56)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val56");
            entity.Property(e => e.Val57)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val57");
            entity.Property(e => e.Val58)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val58");
            entity.Property(e => e.Val59)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val59");
            entity.Property(e => e.Val6)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val6");
            entity.Property(e => e.Val60)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val60");
            entity.Property(e => e.Val61)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val61");
            entity.Property(e => e.Val62)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val62");
            entity.Property(e => e.Val63)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val63");
            entity.Property(e => e.Val64)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val64");
            entity.Property(e => e.Val65)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val65");
            entity.Property(e => e.Val66)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val66");
            entity.Property(e => e.Val67)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val67");
            entity.Property(e => e.Val68)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val68");
            entity.Property(e => e.Val69)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val69");
            entity.Property(e => e.Val7)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val7");
            entity.Property(e => e.Val70)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val70");
            entity.Property(e => e.Val71)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val71");
            entity.Property(e => e.Val72)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val72");
            entity.Property(e => e.Val73)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val73");
            entity.Property(e => e.Val74)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val74");
            entity.Property(e => e.Val75)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val75");
            entity.Property(e => e.Val76)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val76");
            entity.Property(e => e.Val77)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val77");
            entity.Property(e => e.Val78)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val78");
            entity.Property(e => e.Val79)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val79");
            entity.Property(e => e.Val8)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val8");
            entity.Property(e => e.Val80)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val80");
            entity.Property(e => e.Val81)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val81");
            entity.Property(e => e.Val82)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val82");
            entity.Property(e => e.Val83)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val83");
            entity.Property(e => e.Val84)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val84");
            entity.Property(e => e.Val85)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val85");
            entity.Property(e => e.Val86)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val86");
            entity.Property(e => e.Val87)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val87");
            entity.Property(e => e.Val88)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val88");
            entity.Property(e => e.Val89)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val89");
            entity.Property(e => e.Val9)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val9");
            entity.Property(e => e.Val90)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val90");
            entity.Property(e => e.Val91)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val91");
            entity.Property(e => e.Val92)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val92");
            entity.Property(e => e.Val93)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val93");
            entity.Property(e => e.Val94)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val94");
            entity.Property(e => e.Val95)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val95");
            entity.Property(e => e.Val96)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val96");
            entity.Property(e => e.Val97)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val97");
            entity.Property(e => e.Val98)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val98");
            entity.Property(e => e.Val99)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("val99");
        });

        modelBuilder.Entity<TestParameterInspection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestPara__3214EC0798947EBD");

            entity.ToTable("TestParameterInspection");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EnglishName)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Max).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Min).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.OptionSegment)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TypeInput)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Watermark)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestParameterInspectionClassifier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestPara__3214EC0729B278D5");

            entity.ToTable("TestParameterInspectionClassifier");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ClassifierL1).HasColumnName("Classifier_L1");
            entity.Property(e => e.ClassifierL2).HasColumnName("Classifier_L2");
            entity.Property(e => e.ClassifierL3).HasColumnName("Classifier_L3");
            entity.Property(e => e.ClassifierNameL1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L1");
            entity.Property(e => e.ClassifierNameL2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L2");
            entity.Property(e => e.ClassifierNameL3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L3");
            entity.Property(e => e.ParameterId).HasColumnName("Parameter_Id");
            entity.Property(e => e.ParameterName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestRequ__3214EC07313AED6A");

            entity.ToTable("TestRequest");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.AssemblyMoisture).HasColumnType("decimal(8, 1)");
            entity.Property(e => e.AssemblyTemperature).HasColumnType("decimal(8, 1)");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.Customer)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Evaluation).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.KeyWord)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Link)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.MaterialSap)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NewTest)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NormaId).HasColumnName("Norma_Id");
            entity.Property(e => e.Observation)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PartDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PartNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProjectName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reference)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SampleDateBatch)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SampleDate_Batch");
            entity.Property(e => e.SizeProbeta)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StackCode)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.StackInfo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Stack_Info");
            entity.Property(e => e.Standard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("standard");
            entity.Property(e => e.Status)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.TagId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tag_Id");
            entity.Property(e => e.TechnicalId).HasColumnName("Technical_Id");
            entity.Property(e => e.TotalDuration).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
        });

        modelBuilder.Entity<TestRequestClassifier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestRequ__3214EC0748838673");

            entity.ToTable("TestRequestClassifier");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestRequestFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestRequ__3214EC07F21F5AE1");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.File1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File10)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File4)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File5)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File6)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File8)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.File9)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TestParameterInspectionId).HasColumnName("TestParameterInspection_Id");
            entity.Property(e => e.TestParameterName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TestRequestId).HasColumnName("TestRequest_Id");
            entity.Property(e => e.TypeFile)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestRequestMeasurement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestRequ__3214EC077FB16E08");

            entity.ToTable("TestRequestMeasurement");

            entity.Property(e => e.ClassifierL1).HasColumnName("Classifier_L1");
            entity.Property(e => e.ClassifierL2).HasColumnName("Classifier_L2");
            entity.Property(e => e.ClassifierL3).HasColumnName("Classifier_L3");
            entity.Property(e => e.ClassifierNameL1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L1");
            entity.Property(e => e.ClassifierNameL2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L2");
            entity.Property(e => e.ClassifierNameL3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Created_By");
            entity.Property(e => e.DateValue)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Hora)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InputType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Keyword)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Link)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Max).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Min).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Observation)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ParameterClassifierId).HasColumnName("ParameterClassifier_Id");
            entity.Property(e => e.ParameterName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParameterSegment)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TestParameterInspectionId).HasColumnName("TestParameterInspection_Id");
            entity.Property(e => e.TestRequestId).HasColumnName("TestRequest_Id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Updated_By");
            entity.Property(e => e.Val1)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Val10)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val11)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val12)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val13)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val14)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val15)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val16)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val17)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val18)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val19)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val20)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val21)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val22)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val23)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val24)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val25)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val4)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val5)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val6)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val7)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val8)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val9)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestRequestParameterInspection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestRequ__3214EC075F5C4F50");

            entity.ToTable("TestRequestParameterInspection");

            entity.Property(e => e.ClassifierL1).HasColumnName("Classifier_L1");
            entity.Property(e => e.ClassifierL2).HasColumnName("Classifier_L2");
            entity.Property(e => e.ClassifierL3).HasColumnName("Classifier_L3");
            entity.Property(e => e.ClassifierNameL1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L1");
            entity.Property(e => e.ClassifierNameL2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L2");
            entity.Property(e => e.ClassifierNameL3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L3");
            entity.Property(e => e.ParameterClassifierId).HasColumnName("ParameterClassifier_Id");
            entity.Property(e => e.ParameterName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TestParameterInspectionId).HasColumnName("TestParameterInspection_Id");
            entity.Property(e => e.TestRequestId).HasColumnName("TestRequest_Id");
        });

        modelBuilder.Entity<TestRequestParameterInspectionTmp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TestRequestParameterInspection_Tmp");

            entity.Property(e => e.ParameterClassifierId).HasColumnName("ParameterClassifier_Id");
            entity.Property(e => e.TestParameterInspectionId).HasColumnName("TestParameterInspection_Id");
            entity.Property(e => e.TestRequestId).HasColumnName("TestRequest_Id");
        });

        modelBuilder.Entity<TestRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestRequ__3214EC07B76455CD");

            entity.ToTable("TestRequestStatus");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Hierarchy).HasDefaultValueSql("((0))");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TestRequestStatusHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TestRequestStatusHistory");

            entity.Property(e => e.DateStatus).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.StatusCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Status_Code");
            entity.Property(e => e.TestRequestId).HasColumnName("TestRequest_Id");
        });

        modelBuilder.Entity<Testdatum>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.OtherId).HasColumnName("OtherID");
            entity.Property(e => e.SomeId).HasColumnName("SomeID");
            entity.Property(e => e.String).IsUnicode(false);
        });

        modelBuilder.Entity<TypeEssay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeEssa__3214EC072CAD2508");

            entity.ToTable("TypeEssay");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NameEnglish)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserCentro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.UserCentro");

            entity.ToTable("UserCentro");
        });

        modelBuilder.Entity<UserModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.UserModule");

            entity.ToTable("UserModule");
        });

        modelBuilder.Entity<UserSystem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.UserSystem");

            entity.ToTable("UserSystem");
        });

        modelBuilder.Entity<VbiproductControlTissue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VBIProductControlTissue");

            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VprocessDefect>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VProcessDefects");

            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Defect)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DefectId).HasColumnName("Defect_Id");
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VprocessProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VProcessProducts");

            entity.Property(e => e.Centro)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.ProcessId).HasColumnName("Process_Id");
            entity.Property(e => e.ProcessName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProductId).HasColumnName("Product_Id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VprofileModule>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Name }).HasName("PK_dbo.VProfileModules");

            entity.ToTable("VProfileModules");

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.Parent).HasMaxLength(200);
        });

        modelBuilder.Entity<VwParameterInspection>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_PARAMETER_INSPECTION");

            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EnglishName)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IndexWm).HasColumnName("IndexWM");
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Value)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("value");
            entity.Property(e => e.Watermark)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwTestRequestMeasurement>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_TEST_REQUEST_MEASUREMENTS");

            entity.Property(e => e.ClassifierNameL1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L1");
            entity.Property(e => e.ClassifierNameL2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L2");
            entity.Property(e => e.ClassifierNameL3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L3");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.KeyWordTr)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("KeyWordTR");
            entity.Property(e => e.Keyword)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Observation)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ParameterName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Pmrequester)
                .HasMaxLength(161)
                .IsUnicode(false)
                .HasColumnName("PMRequester");
            entity.Property(e => e.StackCode)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.StackInfo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Stack_Info");
            entity.Property(e => e.Status)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.TagId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tag_Id");
            entity.Property(e => e.Technician)
                .HasMaxLength(161)
                .IsUnicode(false);
            entity.Property(e => e.Test)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TotalDuration).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Val1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val10)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val4)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val5)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val6)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val7)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val8)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Val9)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwTestRequestMeasurementsUnpivot>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VW_TEST_REQUEST_MEASUREMENTS_UNPIVOT");

            entity.Property(e => e.ClassifierNameL1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L1");
            entity.Property(e => e.ClassifierNameL2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L2");
            entity.Property(e => e.ClassifierNameL3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ClassifierName_L3");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Field).HasMaxLength(128);
            entity.Property(e => e.IndexField).HasMaxLength(4000);
            entity.Property(e => e.KeyWordTr)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("KeyWordTR");
            entity.Property(e => e.Keyword)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParameterName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Pmrequester)
                .HasMaxLength(161)
                .IsUnicode(false)
                .HasColumnName("PMRequester");
            entity.Property(e => e.StackCode)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.StackInfo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Stack_Info");
            entity.Property(e => e.Status)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.TagId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tag_Id");
            entity.Property(e => e.Technician)
                .HasMaxLength(161)
                .IsUnicode(false);
            entity.Property(e => e.Test)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.TotalDuration).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Val)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
