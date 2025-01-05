using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Roles;

public interface IRoleWriteOnlyRepository
{
    Task Add(Role role);
    Task<Role?> GetById(Guid id);
    void Delete(Role role);
}