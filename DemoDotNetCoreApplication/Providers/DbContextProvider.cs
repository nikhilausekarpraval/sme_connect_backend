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
                entity.HasKey(e => e.Id); // Primary key
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MobileNo).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedOnDt).HasDefaultValueSql("GETDATE()");
                entity.HasMany(e => e.taskItems)
                .WithOne(t => t.employee)
                .HasForeignKey(t => t.employeeId)
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<TaskItem>().ToTable("taskItem");

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id); // Primary key
                entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
                entity.Property(t => t.Description).HasMaxLength(500);
                entity.Property(t => t.CreatedOnDt).HasDefaultValueSql("GETDATE()");
                entity.HasOne(t => t.employee).WithMany(t => t.taskItems) 
                .HasForeignKey(t => t.employeeId) 
                .OnDelete(DeleteBehavior.Cascade); 
        });
        }
    }

}
