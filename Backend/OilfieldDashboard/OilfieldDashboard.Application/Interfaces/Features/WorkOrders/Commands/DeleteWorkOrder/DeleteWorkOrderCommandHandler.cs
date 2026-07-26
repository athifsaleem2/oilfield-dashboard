using MediatR;
using Microsoft.EntityFrameworkCore;
using OilfieldDashboard.Application.Common.Exceptions;
using OilfieldDashboard.Application.Interfaces;
using OilfieldDashboard.Domain.Entities;

namespace OilfieldDashboard.Application.Features.WorkOrders.Commands.DeleteWorkOrder
{
    public class DeleteWorkOrderCommandHandler : IRequestHandler<DeleteWorkOrderCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteWorkOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (workOrder is null)
                throw new NotFoundException(nameof(WorkOrder), request.Id);

            _context.WorkOrders.Remove(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}