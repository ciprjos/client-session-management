using Application.DTOs;
using FluentValidation;

namespace Application.Validators;
public sealed class FilterSessionValidator : AbstractValidator<FilterSession>
{
    public FilterSessionValidator()
    {
        RuleFor(x => x.ClientName)
            .NotEmpty().WithMessage("Filter cannot be empty.")
            .MaximumLength(100).WithMessage("Filter cannot exceed 100 characters.");
    }
}
