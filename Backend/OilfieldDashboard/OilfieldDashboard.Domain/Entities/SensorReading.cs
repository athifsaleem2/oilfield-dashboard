using System;

namespace OilfieldDashboard.Domain.Entities
{
    public class SensorReading
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public Well Well { get; set; } = null!;
        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double FlowRate { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}