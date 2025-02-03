using GigAuth.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace GigAuth.Infrastructure.Security.Cryptography;

public class Cryptography : ICryptography
{
    public string Encrypt(string value)
    {
        return BC.HashPassword(value);
    }

    public bool Verify(string value, string hashedValue)
    {
        return BC.Verify(value, hashedValue);
    }
}