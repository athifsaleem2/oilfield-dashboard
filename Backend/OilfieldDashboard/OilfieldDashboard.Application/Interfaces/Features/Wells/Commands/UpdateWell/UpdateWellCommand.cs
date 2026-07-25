using MediatR;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.Wells.Commands.UpdateWell
{
    public class UpdateWellCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public WellStatus Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}