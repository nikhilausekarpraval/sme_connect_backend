using System;
using System.Collections.Generic;
using System.Security.Claims;
using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoDotNetCoreApplication.Data;

public partial class DcimDevContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DcimDevContext(DbContextOptions<DcimDevContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<DemoDotNetCoreApplication.Modals.Task> Tasks { get; set; }

    public override DbSet<ApplicationUser> Users { get; set; }

    public virtual DbSet<Practice> Practices { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public override DbSet<ApplicationRole> Roles { get; set; }

    public virtual DbSet<Questions> Questions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditableEntity>();

        var currentUserEmail = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
            {
                entry.Entity.ModifiedOnDt = DateTime.UtcNow;
                entry.Entity.ModifiedBy = currentUserEmail; 
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOnDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.GroupsId).HasColumnName("Groups_id");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOnDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.PracticeId).HasColumnName("practice_id");
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.Groups).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.GroupsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AspNetUsers_Groups");

            entity.HasOne(d => d.Practice).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.PracticeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_AspNetUsers_Practices");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__employee__3213E83FB92F5291");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOnDt).HasColumnName("created_on_dt");
            entity.Property(e => e.Designation)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("designation");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mobile_no");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Position)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("position");
        });

        modelBuilder.Entity<Practice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Practice__3213E83F14691F14");

            entity.ToTable("Practice");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOnDt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F1C7B349F");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AnswerHash).HasColumnName("answerHash");
            entity.Property(e => e.Question1)
                .HasMaxLength(500)
                .HasColumnName("question");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Questions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Questions_UserId");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__task__3213E83F05BE6225");

            entity.ToTable("task");

            entity.HasIndex(e => e.EmployeeId, "IX_task_employee_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignedOnDt).HasColumnName("assigned_on_dt");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOnDt).HasColumnName("created_on_dt");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Employee).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FKmeqi2abtbehx871tag4op3hag");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Groups__3213E83F4104FC2B");

            entity.ToTable("UserGroup");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOnDt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
