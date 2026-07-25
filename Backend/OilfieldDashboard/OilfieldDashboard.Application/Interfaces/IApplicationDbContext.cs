// Application/Interfaces/IApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Well> Wells { get; }
        DbSet<WorkOrder> WorkOrders { get; }
        DbSet<SensorReading> SensorReadings { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}