using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using Moq;

namespace CommonTestsUtilities.Repositories.Auth;

public class ForgotPasswordTokenWriteOnlyRepositoryBuilder
{
    private readonly Mock<IForgotPasswordTokenWriteOnlyRepository> _repository = new();
    
    public IForgotPasswordTokenWriteOnlyRepository Build() => _repository.Object;
}