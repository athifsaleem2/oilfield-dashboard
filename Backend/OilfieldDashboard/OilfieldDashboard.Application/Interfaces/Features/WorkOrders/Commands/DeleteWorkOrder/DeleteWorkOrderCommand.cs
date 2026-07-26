using MediatR;

namespace OilfieldDashboard.Application.Features.WorkOrders.Commands.DeleteWorkOrder
{
    public record DeleteWorkOrderCommand(int Id) : IRequest<Unit>;
}