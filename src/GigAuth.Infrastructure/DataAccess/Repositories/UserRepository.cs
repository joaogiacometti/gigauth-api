using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.Users;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class UserRepository(GigAuthContext dbContext) : IWriteOnlyUserRepository
{
    public async Task Add(User user)
    {
        await dbContext.Users.AddAsync(user);
    }
}