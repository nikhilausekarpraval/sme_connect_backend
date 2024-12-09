using System;
using System.Collections.Generic;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoDotNetCoreApplication.Data;

public partial class DcimDevContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
{
    public DcimDevContext()
    {
    }

    public DcimDevContext(DbContextOptions<DcimDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<DemoDotNetCoreApplication.Modals.Task> Tasks { get; set; }

    public   override  DbSet<ApplicationUser>  Users { get; set; }

    public virtual DbSet<Practice> Practices { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public override DbSet<ApplicationRole> Roles { get; set; }

    public virtual DbSet<Questions> Questions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<DemoDotNetCoreApplication.Modals.Task>(entity =>
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

        //

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.DisplayName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Groups_id).HasColumnName("Groups_id");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOnDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.Practice_id).HasColumnName("practice_id");
            entity.Property(e => e.UserName).HasMaxLength(256);

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
