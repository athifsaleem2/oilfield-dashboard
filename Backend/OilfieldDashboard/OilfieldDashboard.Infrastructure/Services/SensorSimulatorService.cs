using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OilfieldDashboard.Application.Features.SensorReadings;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;
using OilfieldDashboard.Infrastructure.Hubs;

namespace OilfieldDashboard.Infrastructure.Services
{
    public class SensorSimulatorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<MonitorHub> _hubContext;
        private static readonly Random _random = new();

        public SensorSimulatorService(IServiceScopeFactory scopeFactory, IHubContext<MonitorHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(3));

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                var wells = context.Wells.ToList();
                if (wells.Count == 0) continue;

                var broadcastPayload = new List<SensorReadingBroadcastDto>();

                foreach (var well in wells)
                {
                    var reading = new SensorReading
                    {
                        WellId = well.Id,
                        Pressure = Math.Round(2000 + _random.NextDouble() * 500, 2),
                        Temperature = Math.Round(150 + _random.NextDouble() * 50, 2),
                        FlowRate = Math.Round(100 + _random.NextDouble() * 400, 2),
                        Timestamp = DateTime.UtcNow,
                    };

                    context.SensorReadings.Add(reading);
                    broadcastPayload.Add(new SensorReadingBroadcastDto
                    {
                        WellId = reading.WellId,
                        Pressure = reading.Pressure,
                        Temperature = reading.Temperature,
                        FlowRate = reading.FlowRate,
                        Timestamp = reading.Timestamp,
                    });
                }

                await context.SaveChangesAsync(stoppingToken);

                await _hubContext.Clients.All.SendAsync("ReceiveSensorReadings", broadcastPayload, cancellationToken: stoppingToken);
            }
        }
    }
}