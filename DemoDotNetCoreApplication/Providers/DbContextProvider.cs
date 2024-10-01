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
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().ToTable("employee");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.id); // Primary key
                entity.Property(e => e.name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.mobileNo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.createdOnDt).HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.tasks)
                .WithOne(t => t.employee)
                .HasForeignKey(t => t.employeeId)
                .OnDelete(DeleteBehavior.SetNull);

            });

            modelBuilder.Entity<TaskItem>().ToTable("task");

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.id); // Primary key
                entity.Property(t => t.name).IsRequired().HasMaxLength(100);
                entity.Property(t => t.description).HasMaxLength(500);
                entity.Property(t => t.createdOnDt).HasDefaultValueSql("GETDATE()");
                entity.HasOne(t => t.employee).WithMany(t => t.tasks) 
                .HasForeignKey(t => t.employeeId) 
                .OnDelete(DeleteBehavior.SetNull); 
            });
        }
    }

}
