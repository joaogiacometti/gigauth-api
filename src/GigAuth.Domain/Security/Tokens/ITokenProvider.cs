using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Security.Tokens;

public interface ITokenProvider
{
    public string GenerateToken(User user);
    public string? GetUserIdByToken(string token, bool validateLifetime = true);
    public RefreshToken GenerateRefreshToken(Guid userId);
    public ForgotPasswordToken GenerateForgetPasswordToken(User user);
}