using MediatR;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.Wells.Commands.CreateWell
{
    public class CreateWellCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public WellStatus Status { get; set; } = WellStatus.Active;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}