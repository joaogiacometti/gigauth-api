using GigAuth.Domain.Entities;
using GigAuth.Domain.Security.Tokens;
using Moq;

namespace CommonTestsUtilities.Security;

public class TokenProviderBuilder
{
    private readonly Mock<ITokenProvider> _tokenProvider = new();

    public TokenProviderBuilder GenerateToken(string token)
    {
        _tokenProvider.Setup(x => x.GenerateRefreshToken(It.IsAny<Guid>())).Returns(new RefreshToken { Token = token });
        return this;
    }

    public TokenProviderBuilder GetUserIdByToken(string? token = null, string? userId = null)
    {
        if (token is not null)
            _tokenProvider.Setup(x => x.GetUserIdByToken(token, false)).Returns(userId ?? Guid.NewGuid().ToString());

        return this;
    }

    public ITokenProvider Build()
    {
        return _tokenProvider.Object;
    }
}