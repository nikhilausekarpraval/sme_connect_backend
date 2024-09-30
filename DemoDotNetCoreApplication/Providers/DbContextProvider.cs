using DemoDotNetCoreApplication.Modals;
using Microsoft.EntityFrameworkCore;

namespace DemoDotNetCoreApplication.Providers
{

public class DbContextProvider : DbContext
    {
        public DbContextProvider(DbContextOptions<DbContextProvider> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().ToTable("employee");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.id); // Primary key
                entity.Property(e => e.name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.mobile_no).IsRequired().HasMaxLength(20);
                entity.Property(e => e.created_on_dt).HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.task_items)
                .WithOne(t => t.employee)
                .HasForeignKey(t => t.employee_id)
                .OnDelete(DeleteBehavior.SetNull);

            });

            modelBuilder.Entity<TaskItem>().ToTable("task");

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.id); // Primary key
                entity.Property(t => t.name).IsRequired().HasMaxLength(100);
                entity.Property(t => t.description).HasMaxLength(500);
                entity.Property(t => t.created_on_dt).HasDefaultValueSql("GETDATE()");
                entity.HasOne(t => t.employee).WithMany(t => t.task_items) 
                .HasForeignKey(t => t.employee_id) 
                .OnDelete(DeleteBehavior.SetNull); 
        });
        }
    }

}
