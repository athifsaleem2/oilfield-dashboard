using MediatR;

namespace OilfieldDashboard.Application.Features.Alerts.ResolveAlert
{
    public record ResolveAlertCommand(int Id) : IRequest<Unit>;
}