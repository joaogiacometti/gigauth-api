using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class UserRepository(GigAuthContext dbContext) : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    public async Task Add(User user) => await dbContext.Users.AddAsync(user);
    
    public Task<List<User>> GetFiltered(RequestUserFilter filter)
    {
        var query = dbContext.Users
            .AsQueryable()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter.UserName))
            query = query.Where(r => EF.Functions.ILike(r.UserName, $"%{filter.UserName}%"));
        
        if (!string.IsNullOrEmpty(filter.Email))
            query = query.Where(r => EF.Functions.ILike(r.Email, $"%{filter.Email}%"));

        if (filter.IsActive.HasValue)
            query = query.Where(u => u.IsActive == filter.IsActive.Value);

        return query.ToListAsync();
    }

    async Task<User?> IUserWriteOnlyRepository.GetById(Guid id) => await dbContext.Users
        .SingleOrDefaultAsync(u => u.Id.Equals(id));
    
    async Task<User?> IUserReadOnlyRepository.GetById(Guid id) => await dbContext.Users
        .AsNoTracking()
        .Include(u => u.UserRoles)
        .ThenInclude(u => u.Role)
        .SingleOrDefaultAsync(u => u.Id.Equals(id));

    public void Delete(User user) => dbContext.Users.Remove(user);

    public async Task<User?> GetByEmail(string email) => await dbContext.Users
        .AsNoTracking()
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .ThenInclude(ur => ur!.RolePermissions)
        .ThenInclude(urp => urp.Permission)
        .SingleOrDefaultAsync(u => u.Email.Equals(email));

    public async Task<User?> GetByUserName(string userName) => await dbContext.Users
        .AsNoTracking()
        .SingleOrDefaultAsync(u => u.UserName.Equals(userName));
}