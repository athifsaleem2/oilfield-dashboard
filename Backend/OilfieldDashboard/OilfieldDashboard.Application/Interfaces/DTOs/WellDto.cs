using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.Wells.Queries.GetAllWells;

public class WellDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Location { get; set; } = default!;
    public WellStatus Status { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}