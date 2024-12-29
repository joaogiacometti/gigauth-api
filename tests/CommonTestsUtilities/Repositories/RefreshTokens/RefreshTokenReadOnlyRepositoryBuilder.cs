using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.RefreshTokens;
using Moq;

namespace CommonTestsUtilities.Repositories.RefreshTokens;

public class RefreshTokenReadOnlyRepositoryBuilder
{
    private readonly Mock<IRefreshTokenReadOnlyRepository> _repository = new();
    
    public RefreshTokenReadOnlyRepositoryBuilder GetByUserId(RefreshToken? refreshToken = null)
    {
        if(refreshToken is not null)
            _repository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).ReturnsAsync(refreshToken);
        
        return this;
    }
    
    public IRefreshTokenReadOnlyRepository Build() => _repository.Object;
}