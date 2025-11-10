using Application.DTOs;
using FluentValidation;

namespace Application.Validators;
internal class UpdateSessionValidator : AbstractValidator<UpdateSessionDto>
{
    public UpdateSessionValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("ClientId is required.");

        RuleFor(x => x.SessionTypeId)
            .NotEmpty().WithMessage("SessionTypeId is required.");

        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("ProviderId is required.");

        RuleFor(x => x.SessionDate)
            .NotEmpty().WithMessage("SessionDate is required.")
            .GreaterThan(DateTime.MinValue).WithMessage("Session Date must be a valid date.");
    }
}