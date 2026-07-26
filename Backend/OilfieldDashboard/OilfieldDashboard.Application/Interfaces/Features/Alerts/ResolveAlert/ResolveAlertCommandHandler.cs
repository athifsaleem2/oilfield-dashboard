using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Common.Exceptions;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.Alerts.ResolveAlert
{
    public class ResolveAlertCommandHandler : IRequestHandler<ResolveAlertCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public ResolveAlertCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ResolveAlertCommand request, CancellationToken cancellationToken)
        {
            var alert = await _context.Alerts.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (alert is null)
                throw new NotFoundException(nameof(Alert), request.Id);

            alert.IsResolved = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}