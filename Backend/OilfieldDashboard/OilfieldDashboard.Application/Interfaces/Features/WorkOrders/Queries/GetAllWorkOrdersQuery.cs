using MediatR;

namespace OilfieldDashboard.Application.Features.WorkOrders.Queries.GetAllWorkOrders
{
    public record GetAllWorkOrdersQuery : IRequest<List<WorkOrderDto>>;
}