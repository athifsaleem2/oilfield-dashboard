namespace OilfieldDashboard.Application.Features.SensorReadings
{
    public class SensorReadingBroadcastDto
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double FlowRate { get; set; }
        public DateTime Timestamp { get; set; }
    }
}