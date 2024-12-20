using GigAuth.Domain.Security.Tokens;
using Moq;

namespace CommonTestsUtilities.Security;

public class TokenProviderBuilder
{
    private readonly Mock<ITokenProvider> _tokenProvider = new ();
    
    public ITokenProvider Build() => _tokenProvider.Object;
}