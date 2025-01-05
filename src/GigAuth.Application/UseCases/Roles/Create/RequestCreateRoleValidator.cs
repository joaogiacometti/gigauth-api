using FluentValidation;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Roles.Create;

public class RequestCreateRoleValidator: AbstractValidator<RequestCreateRole>
{
    public RequestCreateRoleValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY)
            .MinimumLength(3).WithMessage(ResourceErrorMessages.NAME_TOO_SHORT)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_TOO_LONG);
        RuleFor(r => r.Description)
            .NotEmpty().WithMessage(ResourceErrorMessages.DESCRIPTION_EMPTY)
            .MaximumLength(256).WithMessage(ResourceErrorMessages.DESCRIPTION_TOO_LONG)
            .When(r => r.Description != null);
    }
}