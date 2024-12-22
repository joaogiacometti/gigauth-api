using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class UserRepository(GigAuthContext dbContext) : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    public async Task Add(User user)
    {
        await dbContext.Users
            .AddAsync(user);
    }

    public async Task<User?> GetById(Guid id)
    {
        return await dbContext.Users
            .SingleOrDefaultAsync(u => u.Id.Equals(id));
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }

    public void Delete(User user)
    {
        dbContext.Users.Remove(user);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<User?> GetByUserName(string userName)
    {
        return await dbContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.UserName.Equals(userName));
    }
}