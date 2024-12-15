using GigAuth.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace GigAuth.Infrastructure.Security.Cryptography;

public class Cryptography : ICryptography
{
    public string Encrypt(string password) => BC.HashPassword(password);

    public bool Verify(string password, string encryptedPassword) => BC.Verify(password, encryptedPassword);
}