using MediatR;

namespace OilfieldDashboard.Application.Features.Wells.Queries.GetAllWells;

public record GetAllWellsQuery : IRequest<List<WellDto>>;