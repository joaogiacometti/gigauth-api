using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class RefreshTokenRepository(GigAuthContext dbContext): IRefreshTokenWriteOnlyRepository, IRefreshTokenReadOnlyRepository
{
    public async Task Create(RefreshToken refreshToken) => await dbContext.RefreshTokens.AddAsync(refreshToken);

    public async Task<RefreshToken?> GetByUserId(Guid userId) => await dbContext.RefreshTokens
        .AsNoTracking()
        .SingleOrDefaultAsync();

    public void Delete(RefreshToken refreshToken) => dbContext.RefreshTokens.Remove(refreshToken);
}