using System;

namespace OilfieldDashboard.Domain.Entities
{
    public class Alert
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public Well Well { get; set; } = null!;
        public string Metric { get; set; } = string.Empty; // "Pressure", "Temperature", "FlowRate"
        public double Value { get; set; }
        public double Threshold { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsResolved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}