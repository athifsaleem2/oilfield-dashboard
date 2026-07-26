using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Features.Alerts.GetAllAlerts;
using OilfieldDashboard.Application.Interfaces;

namespace OilfieldDashboard.Application.Interfaces.Features.Alerts.GetAllAlerts
{
    public class GetAllAlertsQueryHandler : IRequestHandler<GetAllAlertsQuery, List<AlertDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllAlertsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AlertDto>> Handle(GetAllAlertsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Alerts
                .Include(a => a.Well)
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AlertDto
                {
                    Id = a.Id,
                    WellId = a.WellId,
                    WellName = a.Well.Name,
                    Metric = a.Metric,
                    Value = a.Value,
                    Threshold = a.Threshold,
                    Message = a.Message,
                    IsResolved = a.IsResolved,
                    CreatedAt = a.CreatedAt,
                })
                .ToListAsync(cancellationToken);
        }
    }
}