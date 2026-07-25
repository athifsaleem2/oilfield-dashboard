using FluentValidation;

namespace OilfieldDashboard.Application.Features.Wells.Commands.UpdateWell
{
    public class UpdateWellCommandValidator : AbstractValidator<UpdateWellCommand>
    {
        public UpdateWellCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Location).NotEmpty().MaximumLength(300);
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }
}