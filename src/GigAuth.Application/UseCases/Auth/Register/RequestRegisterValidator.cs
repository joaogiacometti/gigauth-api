using FluentValidation;
using GigAuth.Application.UseCases.Users;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.Register;

public class RequestRegisterValidator : AbstractValidator<RequestRegister>
{
    public RequestRegisterValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty().WithMessage(ResourceErrorMessages.USER_NAME_EMPTY)
            .MinimumLength(8).WithMessage(ResourceErrorMessages.USER_NAME_TOO_SHORT)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.USER_NAME_TOO_LONG);
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID)
            .MaximumLength(256).WithMessage(ResourceErrorMessages.EMAIL_TOO_LONG);
        RuleFor(u => u.Password)
            .SetValidator(new PasswordValidator<RequestRegister>());
        RuleFor(u => u.PasswordConfirmation)
            .Equal(u => u.Password).WithMessage(ResourceErrorMessages.PASSWORD_CONFIRMATION_DOES_NOT_MATCH);
    }
}