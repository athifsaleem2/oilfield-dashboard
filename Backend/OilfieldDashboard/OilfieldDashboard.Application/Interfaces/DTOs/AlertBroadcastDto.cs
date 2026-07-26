namespace OilfieldDashboard.Application.Features.Alerts
{
    public class AlertBroadcastDto
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public string WellName { get; set; } = string.Empty;
        public string Metric { get; set; } = string.Empty;
        public double Value { get; set; }
        public double Threshold { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}