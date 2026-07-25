using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Interfaces;

namespace OilfieldDashboard.Application.Features.Wells.Queries.GetAllWells;

public class GetAllWellsQueryHandler : IRequestHandler<GetAllWellsQuery, List<WellDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllWellsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<WellDto>> Handle(GetAllWellsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Wells
            .Select(w => new WellDto
            {
                Id = w.Id,
                Name = w.Name,
                Location = w.Location,
                Status = w.Status,
                Latitude = w.Latitude,
                Longitude = w.Longitude
            })
            .ToListAsync(cancellationToken);
    }
}