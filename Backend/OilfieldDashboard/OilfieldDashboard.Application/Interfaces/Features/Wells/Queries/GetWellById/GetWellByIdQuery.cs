using MediatR;
using OilfieldDashboard.Application.Features.Wells.Queries.GetAllWells;

namespace OilfieldDashboard.Application.Features.Wells.Queries.GetWellById;

public record GetWellByIdQuery(int Id) : IRequest<WellDto?>;