using MediatR;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.WorkOrders.Commands.UpdateWorkOrder
{
    public class UpdateWorkOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public WorkOrderStatus Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}