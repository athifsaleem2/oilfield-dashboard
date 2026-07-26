using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.WorkOrders.Queries.GetAllWorkOrders
{
    public class WorkOrderDto
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public string WellName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public WorkOrderStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}