using MediatR;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.WorkOrders.Commands.CreateWorkOrder
{
    public class CreateWorkOrderCommand : IRequest<int>
    {
        public int WellId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}