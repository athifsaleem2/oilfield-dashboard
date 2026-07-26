using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Interfaces;

namespace OilfieldDashboard.Application.Features.WorkOrders.Queries.GetAllWorkOrders
{
    public class GetAllWorkOrdersQueryHandler : IRequestHandler<GetAllWorkOrdersQuery, List<WorkOrderDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllWorkOrdersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkOrderDto>> Handle(GetAllWorkOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _context.WorkOrders
                .Include(w => w.Well)
                .OrderByDescending(w => w.CreatedAt)
                .Select(w => new WorkOrderDto
                {
                    Id = w.Id,
                    WellId = w.WellId,
                    WellName = w.Well.Name,
                    Title = w.Title,
                    Description = w.Description,
                    AssignedTo = w.AssignedTo,
                    Status = w.Status,
                    DueDate = w.DueDate,
                    CreatedAt = w.CreatedAt,
                })
                .ToListAsync(cancellationToken);
        }
    }
}