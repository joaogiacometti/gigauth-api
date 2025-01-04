using FluentValidation;
using GigAuth.Communication.Requests;
using GigAuth.Exception.Resources;

namespace GigAuth.Application.UseCases.Auth.RefreshToken;

public class RequestRefreshTokenValidator : AbstractValidator<RequestRefreshToken>
{
    public RequestRefreshTokenValidator()
    {
        RuleFor(u => u.Token)
            .NotEmpty().WithMessage(ResourceErrorMessages.TOKEN_EMPTY);
        RuleFor(u => u.RefreshToken)
            .NotEmpty().WithMessage(ResourceErrorMessages.REFRESH_TOKEN_EMPTY);
    }
}