using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Roles;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class RoleRepository(GigAuthContext dbContext) : IRoleReadOnlyRepository, IRoleWriteOnlyRepository
{
    public async Task Add(Role role) => await dbContext.Roles.AddAsync(role);

    public async Task<Role?> GetById(Guid id) => await dbContext.Roles
        .AsNoTracking()
        .Include(r => r.RolePermissions)
        .ThenInclude(rp => rp.Permission)
        .SingleOrDefaultAsync(r => r.Id.Equals(id));

    public async Task<Role?> GetByName(string name) => await dbContext.Roles
        .AsNoTracking()
        .SingleOrDefaultAsync(r => r.Name.Equals(name));

    public Task<List<Role>> GetFiltered(RequestRoleFilter filter)
    {
        var query = dbContext.Roles
            .AsQueryable()
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(r => EF.Functions.ILike(r.Name, $"%{filter.Name}%"));

        if (!string.IsNullOrEmpty(filter.Description))
            query = query.Where(r =>
                r.Description != null && EF.Functions.ILike(r.Description, $"%{filter.Description}%"));

        if (filter.IsActive.HasValue)
            query = query.Where(r => r.IsActive == filter.IsActive.Value);

        return query.ToListAsync();
    }
}