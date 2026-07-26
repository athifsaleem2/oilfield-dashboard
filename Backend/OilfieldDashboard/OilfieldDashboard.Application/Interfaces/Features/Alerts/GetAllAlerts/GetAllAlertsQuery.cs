using MediatR;
using OilfieldDashboard.Application.Features.Alerts.GetAllAlerts;

namespace OilfieldDashboard.Application.Interfaces.Features.Alerts.GetAllAlerts
{
    public record GetAllAlertsQuery : IRequest<List<AlertDto>>;
}