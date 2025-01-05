using FluentValidation;
using GigAuth.Domain.Filters;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Permissions.GetFiltered;

public class RequestPermissionFilterValidator: AbstractValidator<RequestPermissionFilter>
{
    public RequestPermissionFilterValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_TOO_LONG)
            .When(u => u.Name is not null);
        RuleFor(u => u.Description)
            .NotEmpty().WithMessage(ResourceErrorMessages.DESCRIPTION_EMPTY)
            .MaximumLength(256).WithMessage(ResourceErrorMessages.DESCRIPTION_TOO_LONG)
            .When(u => u.Description is not null);
    }
}