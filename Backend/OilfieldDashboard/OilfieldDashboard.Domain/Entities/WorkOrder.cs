using System;

namespace OilfieldDashboard.Domain.Entities
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public Well Well { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty;
        public WorkOrderStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum WorkOrderStatus { Open, InProgress, Closed }
}