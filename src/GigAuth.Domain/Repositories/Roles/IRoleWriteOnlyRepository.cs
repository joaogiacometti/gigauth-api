using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Roles;

public interface IRoleWriteOnlyRepository
{
    Task Add(Role role);
}