using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using Moq;

namespace CommonTestsUtilities.Repositories.Auth;

public class ForgotPasswordTokenWriteOnlyRepositoryBuilder
{
    private readonly Mock<IForgotPasswordTokenWriteOnlyRepository> _repository = new();

    public ForgotPasswordTokenWriteOnlyRepositoryBuilder GetByToken(ForgotPasswordToken? token = null)
    {
        if (token is not null)
            _repository.Setup(r => r.GetByToken(token.Token)).ReturnsAsync(token);

        return this;
    }

    public IForgotPasswordTokenWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}