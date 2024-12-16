using GigAuth.Domain.Repositories.Users;
using GigAuth.Domain.Security.Cryptography;
using Moq;

namespace CommonTestsUtilities.Security;

public class CryptographyBuilder
{
    private readonly Mock<ICryptography> _cryptograph = new ();
    
    public ICryptography Build() => _cryptograph.Object;
}