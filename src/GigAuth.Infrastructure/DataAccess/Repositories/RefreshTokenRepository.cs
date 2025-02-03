using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class RefreshTokenRepository(GigAuthContext dbContext)
    : IRefreshTokenWriteOnlyRepository, IRefreshTokenReadOnlyRepository
{
    public async Task<RefreshToken?> GetByUserId(Guid userId)
    {
        return await dbContext.RefreshTokens
            .AsNoTracking()
            .SingleOrDefaultAsync();
    }

    public async Task Create(RefreshToken refreshToken)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public void Delete(RefreshToken refreshToken)
    {
        dbContext.RefreshTokens.Remove(refreshToken);
    }
}