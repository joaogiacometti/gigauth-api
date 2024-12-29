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
    public string GenerateToken(User user)
    {
        var secretKey = configuration["Jwt:SecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(GenerateClaims(user)),
            Expires = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("Jwt:ExpirationInSeconds")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }

    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        var token = Guid.NewGuid().ToString("N");

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            UserId = userId,
            ExpirationDate = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("RefreshToken:ExpirationInSeconds"))
        };
    }

    private static List<Claim> GenerateClaims(User user)
    {
        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToList();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        claims.AddRange(roles.Select(role => new Claim("roles", role)));

        claims.AddRange(permissions.Select(permission => new Claim("permissions", permission)));

        return claims;
    }
}