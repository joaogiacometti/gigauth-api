using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GigAuth.Domain.Entities;
using GigAuth.Domain.Security.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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

    public string? GetUserIdByToken(string token, bool validateLifetime = true)
    {
        var isValid = ValidateToken(token, validateLifetime);

        if (!isValid) return null;
        
        var handler = new JwtSecurityTokenHandler();
        
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        return userIdClaim?.Value;
    }
    
    private bool ValidateToken(string token, bool validateLifetime = true)
    {
        var secretKey = configuration["Jwt:SecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = securityKey
        };

        var handler = new JwtSecurityTokenHandler();

        try
        {
            _ = handler.ValidateToken(token, tokenValidationParameters, out _);
        }
        catch (Exception)
        {
            return false;
        }
        
        return true;
    }
    
    public RefreshToken GenerateRefreshToken(Guid userId) => new()
    {
        Id = Guid.NewGuid(),
        Token = GenerateToken(),
        UserId = userId,
        ExpirationDate = DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("RefreshToken:ExpirationInSeconds"))
    };

    public ForgotPasswordToken GenerateForgetPasswordToken(User user) => new()
    {
        Id = Guid.NewGuid(),
        Token = GenerateToken(),
        ExpirationDate =
            DateTime.UtcNow.AddSeconds(configuration.GetValue<int>("ForgotPasswordToken:ExpirationInSeconds")),
        UserId = user.Id
    };

    private static string GenerateToken() => Guid.NewGuid().ToString("N");

    private static List<Claim> GenerateClaims(User user)
    {
        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role!.RolePermissions)
            .Select(rp => rp.Permission!.Name)
            .Distinct()
            .ToList();

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        claims.AddRange(permissions.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}