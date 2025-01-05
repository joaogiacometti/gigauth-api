using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;

namespace GigAuth.Domain.Repositories.Permissions;

public interface IPermissionReadOnlyRepository
{
    Task<Permission?> GetById(Guid id);
    Task<Permission?> GetByName(string name);
    Task<List<Permission>> GetFiltered(RequestPermissionFilter filter);
}