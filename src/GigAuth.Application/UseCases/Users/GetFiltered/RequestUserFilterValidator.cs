using FluentValidation;
using GigAuth.Domain.Filters;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users.GetFiltered;

public class RequestUserFilterValidator: AbstractValidator<RequestUserFilter>
{
    public RequestUserFilterValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty().WithMessage(ResourceErrorMessages.USER_NAME_EMPTY)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.USER_NAME_TOO_LONG)
            .When(u => u.UserName is not null);
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .MaximumLength(256).WithMessage(ResourceErrorMessages.EMAIL_TOO_LONG)
            .When(u => u.Email is not null);
    }
}