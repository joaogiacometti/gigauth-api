using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.RefreshTokens;

public interface IRefreshTokenReadOnlyRepository
{
    public Task<RefreshToken?> GetByUserId(Guid userId);
}