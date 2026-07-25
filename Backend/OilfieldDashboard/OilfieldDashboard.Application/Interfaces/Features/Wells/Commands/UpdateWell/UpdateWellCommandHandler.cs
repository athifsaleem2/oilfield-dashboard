using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Common.Exceptions;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.Wells.Commands.UpdateWell
{
    public class UpdateWellCommandHandler : IRequestHandler<UpdateWellCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateWellCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateWellCommand request, CancellationToken cancellationToken)
        {
            var well = await _context.Wells
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (well is null)
                throw new NotFoundException(nameof(Well), request.Id);

            well.Name = request.Name;
            well.Location = request.Location;
            well.Status = request.Status;
            well.Latitude = request.Latitude;
            well.Longitude = request.Longitude;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}