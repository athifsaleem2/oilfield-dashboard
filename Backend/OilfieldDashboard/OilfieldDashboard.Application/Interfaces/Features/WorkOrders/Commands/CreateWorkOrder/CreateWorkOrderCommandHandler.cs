using MediatR;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.WorkOrders.Commands.CreateWorkOrder
{
    public class CreateWorkOrderCommandHandler : IRequestHandler<CreateWorkOrderCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateWorkOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = new WorkOrder
            {
                WellId = request.WellId,
                Title = request.Title,
                Description = request.Description,
                AssignedTo = request.AssignedTo,
                DueDate = request.DueDate,
                Status = WorkOrderStatus.Open,
            };

            _context.WorkOrders.Add(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return workOrder.Id;
        }
    }
}