using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OilfieldDashboard.Infrastructure.Persistence
{
    /// <summary>
    /// Provides design-time DbContext creation for EF Core CLI tools (migrations, database update).
    /// Uses a hardcoded LocalDB connection string so the CLI doesn't need the full app startup.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=OilfieldDashboardDb;Trusted_Connection=True;MultipleActiveResultSets=true",
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
