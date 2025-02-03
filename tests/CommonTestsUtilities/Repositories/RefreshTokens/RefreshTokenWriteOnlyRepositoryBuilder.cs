using GigAuth.Domain.Repositories.RefreshTokens;
using Moq;

namespace CommonTestsUtilities.Repositories.RefreshTokens;

public class RefreshTokenWriteOnlyRepositoryBuilder
{
    private readonly Mock<IRefreshTokenWriteOnlyRepository> _repository = new();

    public IRefreshTokenWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}