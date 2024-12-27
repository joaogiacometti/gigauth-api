using GigAuth.Domain.Repositories;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class UnitOfWork(GigAuthContext dbContext) : IUnitOfWork
{
    public async Task Commit() => await dbContext.SaveChangesAsync();
}