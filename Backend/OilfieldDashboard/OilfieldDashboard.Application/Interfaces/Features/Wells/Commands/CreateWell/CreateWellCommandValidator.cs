using FluentValidation;

namespace OilfieldDashboard.Application.Features.Wells.Commands.CreateWell
{
    public class CreateWellCommandValidator : AbstractValidator<CreateWellCommand>
    {
        public CreateWellCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Well name is required.")
                .MaximumLength(200);

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(300);

            RuleFor(x => x.Status)
                .IsInEnum();

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }
}