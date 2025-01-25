using FluentValidation;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Users.Update;

public class RequestUpdateUserValidator: AbstractValidator<RequestUpdateUser>
{
    public RequestUpdateUserValidator()
    {
        RuleFor(u => u.UserName)
            .MinimumLength(8).WithMessage(ResourceErrorMessages.USER_NAME_TOO_SHORT)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.USER_NAME_TOO_LONG);
        RuleFor(u => u.Email)
            .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID)
            .MaximumLength(256).WithMessage(ResourceErrorMessages.EMAIL_TOO_LONG);
    }
}