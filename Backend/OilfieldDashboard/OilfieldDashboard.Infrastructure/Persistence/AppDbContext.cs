using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IApplicationDbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Well> Wells => Set<Well>();
        public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Well>()
                .HasMany(w => w.SensorReadings)
                .WithOne(s => s.Well)
                .HasForeignKey(s => s.WellId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkOrder>()
                .HasOne(wo => wo.Well)
                .WithMany()
                .HasForeignKey(wo => wo.WellId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
