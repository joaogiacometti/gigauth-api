namespace GigAuth.Domain.Security.Cryptography;

public interface ICryptography
{
    string Encrypt(string value);
    bool Verify(string value, string hashedValue);
}