using FluentValidation;

namespace OilfieldDashboard.Application.Features.WorkOrders.Commands.CreateWorkOrder
{
    public class CreateWorkOrderCommandValidator : AbstractValidator<CreateWorkOrderCommand>
    {
        public CreateWorkOrderCommandValidator()
        {
            RuleFor(x => x.WellId).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.AssignedTo).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DueDate).GreaterThan(DateTime.UtcNow.Date).WithMessage("Due date must be in the future.");
        }
    }
}