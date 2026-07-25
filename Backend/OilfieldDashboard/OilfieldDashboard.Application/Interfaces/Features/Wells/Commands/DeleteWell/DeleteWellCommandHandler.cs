// Application/Features/Wells/Commands/DeleteWell/DeleteWellCommandHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Common.Exceptions;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.Wells.Commands.DeleteWell
{
    public class DeleteWellCommandHandler : IRequestHandler<DeleteWellCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteWellCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWellCommand request, CancellationToken cancellationToken)
        {
            var well = await _context.Wells
                .Include(w => w.SensorReadings)
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (well is null)
                throw new NotFoundException(nameof(Well), request.Id);

            _context.Wells.Remove(well);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}