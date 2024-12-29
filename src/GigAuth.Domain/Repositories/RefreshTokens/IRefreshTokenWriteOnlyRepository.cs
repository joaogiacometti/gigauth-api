using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.RefreshTokens;

public interface IRefreshTokenWriteOnlyRepository
{
    public Task Create(RefreshToken refreshToken);
    public void Delete(RefreshToken refreshToken);
}