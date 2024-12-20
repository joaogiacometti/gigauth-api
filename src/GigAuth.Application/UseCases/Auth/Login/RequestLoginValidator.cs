using FluentValidation;
using GigAuth.Application.UseCases.Users;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.Login;

public class RequestLoginValidator : AbstractValidator<RequestLogin>
{
    public RequestLoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID)
            .MaximumLength(256).WithMessage(ResourceErrorMessages.EMAIL_TOO_LONG);
        RuleFor(u => u.Password)
            .SetValidator(new PasswordValidator<RequestLogin>());
    }
}