using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.ForgotPasswordTokens;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class ForgotPasswordTokenRepository(GigAuthContext dbContext) : IForgotPasswordTokenReadOnlyRepository, IForgotPasswordTokenWriteOnlyRepository
{
    public async Task Create(ForgotPasswordToken token)
    {
        await dbContext.ForgotPasswordTokens.AddAsync(token);
    }
    
    public async Task<ForgotPasswordToken?> GetByToken(string token)
    {
        return await dbContext.ForgotPasswordTokens
            .SingleOrDefaultAsync(fpt => fpt.Token.Equals(token));
    }

    public async Task<ForgotPasswordToken?> GetByUserId(Guid userId)
    {
        return await dbContext.ForgotPasswordTokens
            .AsNoTracking()
            .SingleOrDefaultAsync(fpt => fpt.UserId == userId);
    }

    public void Update(ForgotPasswordToken token)
    {
        dbContext.ForgotPasswordTokens.Update(token);
    }

    public void Delete(ForgotPasswordToken token)
    {
        dbContext.ForgotPasswordTokens.Remove(token);
    }
}