using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Wells.Any())
            {
                context.Wells.AddRange(
                    new Well
                    {
                        Name = "Alpha-01",
                        Location = "Sector A-1",
                        Status = WellStatus.Active,
                        Latitude = 29.3759,
                        Longitude = 47.9774,
                        MaxPressure = 2450,
                        MaxTemperature = 190,
                        MinFlowRate = 150
                    },
                    new Well
                    {
                        Name = "Beta-02",
                        Location = "Sector A-2",
                        Status = WellStatus.Active,
                        Latitude = 29.3812,
                        Longitude = 47.9821,
                        MaxPressure = 2400,
                        MaxTemperature = 185,
                        MinFlowRate = 160
                    },
                    new Well
                    {
                        Name = "Gamma-03",
                        Location = "Sector B-1",
                        Status = WellStatus.Maintenance,
                        Latitude = 29.3690,
                        Longitude = 47.9650,
                        MaxPressure = 2500,
                        MaxTemperature = 195,
                        MinFlowRate = 140
                    },
                    new Well
                    {
                        Name = "Delta-04",
                        Location = "Sector B-2",
                        Status = WellStatus.Active,
                        Latitude = 29.3722,
                        Longitude = 47.9710,
                        MaxPressure = 2420,
                        MaxTemperature = 188,
                        MinFlowRate = 155
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
