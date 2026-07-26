namespace OilfieldDashboard.Domain.Entities
{
    public class Well
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public WellStatus Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MaxPressure { get; set; } = 2450;
        public double MaxTemperature { get; set; } = 190;
        public double MinFlowRate { get; set; } = 150;
        public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
    }

    public enum WellStatus { Active, Inactive, Maintenance }
}