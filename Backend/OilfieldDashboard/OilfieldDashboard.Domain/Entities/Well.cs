using System;
using System.Collections.Generic;

namespace OilfieldDashboard.Domain.Entities
{
    public class Well
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public WellStatus Status { get; set; }
        public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();
    }

    public enum WellStatus { Active, Inactive, Maintenance }
}