using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Roles;
using GigAuth.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class RoleRepository(GigAuthContext dbContext) : IRoleReadOnlyRepository
{
    public Task<List<Role>> GetFiltered(RequestRoleFilter filter)
    {
        var query = dbContext.Roles
            .AsQueryable()
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(r => r.Name.Contains(filter.Name));

        if (!string.IsNullOrEmpty(filter.Description))
            query = query.Where(r => r.Description != null && r.Description.Contains(filter.Description));

        if (filter.IsActive.HasValue)
            query = query.Where(r => r.IsActive == filter.IsActive.Value);

        return query.ToListAsync();
    }
}