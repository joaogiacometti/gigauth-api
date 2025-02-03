using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Permissions;
using Microsoft.EntityFrameworkCore;

namespace GigAuth.Infrastructure.DataAccess.Repositories;

public class PermissionRepository(GigAuthContext dbContext)
    : IPermissionReadOnlyRepository, IPermissionWriteOnlyRepository
{
    async Task<Permission?> IPermissionReadOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Id.Equals(id));
    }

    public async Task<Permission?> GetByName(string name)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .SingleOrDefaultAsync(r => r.Name.Equals(name));
    }

    public Task<List<Permission>> GetFiltered(RequestPermissionFilter filter)
    {
        var query = dbContext.Permissions
            .AsQueryable()
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

    public async Task Add(Permission permission)
    {
        await dbContext.Permissions.AddAsync(permission);
    }

    public void Delete(Permission permission)
    {
        dbContext.Permissions.Remove(permission);
    }

    async Task<Permission?> IPermissionWriteOnlyRepository.GetById(Guid id)
    {
        return await dbContext.Permissions
            .SingleOrDefaultAsync(r => r.Id.Equals(id));
    }
}