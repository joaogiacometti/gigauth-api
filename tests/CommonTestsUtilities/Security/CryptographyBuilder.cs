using GigAuth.Domain.Security.Cryptography;
using Moq;

namespace CommonTestsUtilities.Security;

public class CryptographyBuilder
{
    private readonly Mock<ICryptography> _cryptography = new();

    public CryptographyBuilder Verify(string? input = null)
    {
        if (input is not null)
            _cryptography.Setup(c => c.Verify(input, It.IsAny<string>())).Returns(true);

        return this;
    }

    public ICryptography Build()
    {
        return _cryptography.Object;
    }
}