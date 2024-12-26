using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
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

    async Task<User?> IUserReadOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Users
            .SingleOrDefaultAsync(u => u.Id.Equals(id));
    }

    public Task<List<User>> GetFiltered(RequestUserFilter filter)
    {
        var query = dbContext.Users
            .AsQueryable()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsNoTracking();
        
        if (!string.IsNullOrEmpty(filter.UserName))
            query = query.Where(u => u.UserName.Contains(filter.UserName));

        if (!string.IsNullOrEmpty(filter.Email))
            query = query.Where(u => u.Email.Contains(filter.Email));

        if (filter.IsActive.HasValue)
            query = query.Where(u => u.IsActive == filter.IsActive.Value);
        
        return query.ToListAsync();
    }

    async Task<User?> IUserWriteOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id.Equals(id));
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