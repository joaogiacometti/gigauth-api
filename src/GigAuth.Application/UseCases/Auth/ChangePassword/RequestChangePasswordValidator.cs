using FluentValidation;
using GigAuth.Application.UseCases.Users;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.ChangePassword;

public class RequestChangePasswordValidator : AbstractValidator<RequestChangePassword>
{
    public RequestChangePasswordValidator()
    {
        RuleFor(u => u.Token)
            .NotEmpty().WithMessage(ResourceErrorMessages.TOKEN_EMPTY);
        RuleFor(u => u.NewPassword)
            .SetValidator(new PasswordValidator<RequestChangePassword>());
    }
}