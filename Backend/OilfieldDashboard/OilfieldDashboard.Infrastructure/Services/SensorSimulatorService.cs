using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OilfieldDashboard.Application.Features.Alerts;
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
        private readonly ILogger<SensorSimulatorService> _logger;
        private static readonly Random _random = new();

        public SensorSimulatorService(
            IServiceScopeFactory scopeFactory,
            IHubContext<MonitorHub> hubContext,
            ILogger<SensorSimulatorService> logger)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SensorSimulatorService started.");

            // Perform initial tick immediately on startup
            await SimulateAndBroadcastAsync(stoppingToken);

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(3));

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                await SimulateAndBroadcastAsync(stoppingToken);
            }
        }

        private async Task SimulateAndBroadcastAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                List<Well> wells = new();
                try
                {
                    wells = context.Wells.ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to query wells from database. Using fallback simulated wells.");
                }

                if (wells.Count == 0)
                {
                    wells = GetFallbackWells();
                    try
                    {
                        foreach (var w in wells)
                        {
                            context.Wells.Add(new Well
                            {
                                Name = w.Name,
                                Location = w.Location,
                                Status = w.Status,
                                Latitude = w.Latitude,
                                Longitude = w.Longitude,
                                MaxPressure = w.MaxPressure,
                                MaxTemperature = w.MaxTemperature,
                                MinFlowRate = w.MinFlowRate
                            });
                        }
                        await context.SaveChangesAsync(stoppingToken);
                        wells = context.Wells.ToList();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Could not save fallback wells to DB.");
                    }
                }

                if (wells.Count == 0)
                {
                    wells = GetFallbackWells();
                }

                var broadcastPayload = new List<SensorReadingBroadcastDto>();
                var newAlerts = new List<Alert>();

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

                    try
                    {
                        context.SensorReadings.Add(reading);
                    }
                    catch { }

                    broadcastPayload.Add(new SensorReadingBroadcastDto
                    {
                        Id = reading.Id,
                        WellId = reading.WellId,
                        Pressure = reading.Pressure,
                        Temperature = reading.Temperature,
                        FlowRate = reading.FlowRate,
                        Timestamp = reading.Timestamp,
                    });

                    if (reading.Pressure > well.MaxPressure && !HasActiveAlert(context, well.Id, "Pressure"))
                    {
                        newAlerts.Add(new Alert
                        {
                            WellId = well.Id,
                            Metric = "Pressure",
                            Value = reading.Pressure,
                            Threshold = well.MaxPressure,
                            Message = $"{well.Name}: Pressure {reading.Pressure} psi exceeds max {well.MaxPressure} psi",
                        });
                    }

                    if (reading.Temperature > well.MaxTemperature && !HasActiveAlert(context, well.Id, "Temperature"))
                    {
                        newAlerts.Add(new Alert
                        {
                            WellId = well.Id,
                            Metric = "Temperature",
                            Value = reading.Temperature,
                            Threshold = well.MaxTemperature,
                            Message = $"{well.Name}: Temperature {reading.Temperature} °F exceeds max {well.MaxTemperature} °F",
                        });
                    }

                    if (reading.FlowRate < well.MinFlowRate && !HasActiveAlert(context, well.Id, "FlowRate"))
                    {
                        newAlerts.Add(new Alert
                        {
                            WellId = well.Id,
                            Metric = "FlowRate",
                            Value = reading.FlowRate,
                            Threshold = well.MinFlowRate,
                            Message = $"{well.Name}: Flow rate {reading.FlowRate} bbl/day below min {well.MinFlowRate} bbl/day",
                        });
                    }
                }

                if (newAlerts.Count > 0)
                {
                    try
                    {
                        context.Alerts.AddRange(newAlerts);
                    }
                    catch { }
                }

                try
                {
                    await context.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to save sensor readings/alerts to database.");
                }

                await _hubContext.Clients.All.SendAsync("ReceiveSensorReadings", broadcastPayload, cancellationToken: stoppingToken);
                _logger.LogInformation("Broadcasted {Count} sensor readings to SignalR clients.", broadcastPayload.Count);

                if (newAlerts.Count > 0)
                {
                    var alertDtos = newAlerts.Select(a => new AlertBroadcastDto
                    {
                        Id = a.Id,
                        WellId = a.WellId,
                        WellName = wells.FirstOrDefault(w => w.Id == a.WellId)?.Name ?? $"Well #{a.WellId}",
                        Metric = a.Metric,
                        Value = a.Value,
                        Threshold = a.Threshold,
                        Message = a.Message,
                        CreatedAt = a.CreatedAt,
                    }).ToList();

                    await _hubContext.Clients.All.SendAsync("ReceiveAlerts", alertDtos, cancellationToken: stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SensorSimulatorService tick execution.");
            }
        }

        private bool HasActiveAlert(IApplicationDbContext context, int wellId, string metric)
        {
            try
            {
                return context.Alerts.Any(a => a.WellId == wellId && a.Metric == metric && !a.IsResolved);
            }
            catch
            {
                return false;
            }
        }

        private static List<Well> GetFallbackWells()
        {
            return new List<Well>
            {
                new Well { Id = 1, Name = "Alpha-01", Location = "Sector A-1", Status = WellStatus.Active, MaxPressure = 2450, MaxTemperature = 190, MinFlowRate = 150 },
                new Well { Id = 2, Name = "Beta-02", Location = "Sector A-2", Status = WellStatus.Active, MaxPressure = 2400, MaxTemperature = 185, MinFlowRate = 160 },
                new Well { Id = 3, Name = "Gamma-03", Location = "Sector B-1", Status = WellStatus.Maintenance, MaxPressure = 2500, MaxTemperature = 195, MinFlowRate = 140 },
                new Well { Id = 4, Name = "Delta-04", Location = "Sector B-2", Status = WellStatus.Active, MaxPressure = 2420, MaxTemperature = 188, MinFlowRate = 155 },
            };
        }
    }
}