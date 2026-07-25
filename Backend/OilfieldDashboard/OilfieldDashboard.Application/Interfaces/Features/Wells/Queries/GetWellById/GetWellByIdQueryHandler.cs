using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Features.Wells.Queries.GetAllWells;
using OilfieldDashboard.Application.Interfaces;

namespace OilfieldDashboard.Application.Features.Wells.Queries.GetWellById;

public class GetWellByIdQueryHandler : IRequestHandler<GetWellByIdQuery, WellDto?>
{
    private readonly IApplicationDbContext _context;

    public GetWellByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WellDto?> Handle(GetWellByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Wells
            .Where(w => w.Id == request.Id)
            .Select(w => new WellDto
            {
                Id = w.Id,
                Name = w.Name,
                Location = w.Location,
                Status = w.Status,
                Latitude = w.Latitude,
                Longitude = w.Longitude
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}