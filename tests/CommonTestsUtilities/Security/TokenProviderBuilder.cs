using GigAuth.Domain.Entities;
using GigAuth.Domain.Security.Tokens;
using Moq;

namespace CommonTestsUtilities.Security;

public class TokenProviderBuilder
{
    private readonly Mock<ITokenProvider> _tokenProvider = new ();
    
    public TokenProviderBuilder GenerateToken(string token) 
    {
        _tokenProvider.Setup(x => x.GenerateRefreshToken(It.IsAny<Guid>())).Returns(new RefreshToken { Token = token });
        return this;
    }
    
    public ITokenProvider Build() => _tokenProvider.Object;
}