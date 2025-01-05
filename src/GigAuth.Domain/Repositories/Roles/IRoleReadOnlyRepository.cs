using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;

namespace GigAuth.Domain.Repositories.Roles;

public interface IRoleReadOnlyRepository
{
    Task<List<Role>> GetFiltered(RequestRoleFilter filter);
}