using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;

namespace GigAuth.Domain.Repositories.Roles;

public interface IRoleReadOnlyRepository
{
    Task<Role?> GetById(Guid id);
    Task<Role?> GetByName(string name);
    Task<List<Role>> GetFiltered(RequestRoleFilter filter);
}