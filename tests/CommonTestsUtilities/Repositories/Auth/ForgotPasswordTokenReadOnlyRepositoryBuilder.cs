using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using Moq;

namespace CommonTestsUtilities.Repositories.Auth;

public class ForgotPasswordTokenReadOnlyRepositoryBuilder
{
    private readonly Mock<IForgotPasswordTokenReadOnlyRepository> _repository = new();

    public ForgotPasswordTokenReadOnlyRepositoryBuilder GetByUserId(ForgotPasswordToken? token = null)
    {
        if (token is not null)
            _repository.Setup(r => r.GetByUserId(It.IsAny<Guid>())).ReturnsAsync(token);
        
        return this;
    }
    
    public IForgotPasswordTokenReadOnlyRepository Build() => _repository.Object;
}