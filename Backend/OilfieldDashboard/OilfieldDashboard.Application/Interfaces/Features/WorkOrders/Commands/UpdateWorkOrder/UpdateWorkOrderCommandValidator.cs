using FluentValidation;

namespace OilfieldDashboard.Application.Features.WorkOrders.Commands.UpdateWorkOrder
{
    public class UpdateWorkOrderCommandValidator : AbstractValidator<UpdateWorkOrderCommand>
    {
        public UpdateWorkOrderCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.AssignedTo).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.DueDate).NotEqual(default(DateTime));
        }
    }
}