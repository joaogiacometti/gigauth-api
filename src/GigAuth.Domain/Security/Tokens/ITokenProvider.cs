using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Security.Tokens;

public interface ITokenProvider
{
    public string GenerateToken(User user);
    public RefreshToken GenerateRefreshToken(Guid userId);
}