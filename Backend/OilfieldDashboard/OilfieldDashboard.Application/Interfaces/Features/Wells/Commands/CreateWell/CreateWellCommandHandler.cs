using MediatR;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.Wells.Commands.CreateWell
{
    public class CreateWellCommandHandler : IRequestHandler<CreateWellCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateWellCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateWellCommand request, CancellationToken cancellationToken)
        {
            var well = new Well
            {
                Name = request.Name,
                Location = request.Location,
                Status = request.Status,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            _context.Wells.Add(well);
            await _context.SaveChangesAsync(cancellationToken);

            return well.Id;
        }
    }
}