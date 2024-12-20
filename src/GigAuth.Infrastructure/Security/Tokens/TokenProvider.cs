using System.Security.Claims;
using System.Text;
using GigAuth.Domain.Entities;
using GigAuth.Domain.Security.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace GigAuth.Infrastructure.Security.Tokens;

public class TokenProvider(IConfiguration configuration) : ITokenProvider 
{
    public string Generate(User user)
    {
        var secretKey = configuration["Jwt:SecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            ]),
            Expires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("Jwt:ExpirationInSeconds")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audiences"],
        };

        var handler = new JsonWebTokenHandler();
        
        return handler.CreateToken(tokenDescriptor);
    }
}