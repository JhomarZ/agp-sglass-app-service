using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AGP.Security.DataAccessLayer;

public partial class AgpSecurityContext : DbContext
{
    public AgpSecurityContext()
    {
    }

    public AgpSecurityContext(DbContextOptions<AgpSecurityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Centro> Centros { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileModule> ProfileModules { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<ReportPbi> ReportPbis { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCentro> UserCentros { get; set; }

    public virtual DbSet<UserModule> UserModules { get; set; }

    public virtual DbSet<UserSystem> UserSystems { get; set; }

    public virtual DbSet<VprofileModule> VprofileModules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=20.195.211.31;Database=AgpSecurity;user id=adminsa;password=AdminS@2021!#;persist security info=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Area__3214EC07380CCA53");

            entity.ToTable("Area");

            entity.Property(e => e.Center)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasDefaultValueSql("('BE02')");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameEng)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Centro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Centro__3214EC079B628A54");

            entity.ToTable("Centro");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3214EC0747A3A249");

            entity.ToTable("Company");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Module__3214EC07F47C1C41");

            entity.ToTable("Module");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Controller)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.EmbedPbi)
                .HasDefaultValueSql("((0))")
                .HasColumnName("EmbedPBI");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsParent).HasDefaultValueSql("((0))");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Sort).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Process__3213E83F343486A0");

            entity.ToTable("Process");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.Center)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("center");
            entity.Property(e => e.Compania)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("compania");
            entity.Property(e => e.Description)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profile__3214EC07BBA9A8FB");

            entity.ToTable("Profile");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProfileModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProfileM__3214EC073F3AD110");

            entity.ToTable("ProfileModule");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07811FA435");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);
        });

        modelBuilder.Entity<ReportPbi>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dasboard__3214EC07E30D8B4E");

            entity.ToTable("ReportPBI");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.AplicationId)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('AGP')");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ReportId)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.WorkspaceId)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Center)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tokens__3214EC07DF16295B");

            entity.HasIndex(e => e.UserId, "UQ__Tokens__1788CC4D0295344C").IsUnique();

            entity.Property(e => e.AccessToken).HasMaxLength(600);
            entity.Property(e => e.AccessTokenExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken).HasMaxLength(600);
            entity.Property(e => e.RefreshTokenExpirationDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.Token)
                .HasForeignKey<Token>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tokens__UserId__29221CFB");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07F6E3FFFF");

            entity.Property(e => e.Active).HasDefaultValueSql("((1))");
            entity.Property(e => e.Centro)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('PE01')");
            entity.Property(e => e.Compania)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('AGP')");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IdHubDepartment).HasColumnName("id_hub_department");
            entity.Property(e => e.IdHubProcess).HasColumnName("id_hub_process");
            entity.Property(e => e.LastAcces).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OfficeAcount)
                .HasDefaultValueSql("((1))")
                .HasColumnName("OfficeACount");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasDefaultValueSql("((0))");
            entity.Property(e => e.Un)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("UN");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Users)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__AreaId__22751F6C");
        });

        modelBuilder.Entity<UserCentro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserCent__3214EC07C4BC73AB");

            entity.ToTable("UserCentro");

            entity.Property(e => e.Centro)
                .HasMaxLength(4)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserModu__3214EC07A6B7335B");

            entity.ToTable("UserModule");
        });

        modelBuilder.Entity<UserSystem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserSyst__3214EC078266FFF0");

            entity.ToTable("UserSystem");
        });

        modelBuilder.Entity<VprofileModule>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VProfileModules");

            entity.Property(e => e.Description)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Parent)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
