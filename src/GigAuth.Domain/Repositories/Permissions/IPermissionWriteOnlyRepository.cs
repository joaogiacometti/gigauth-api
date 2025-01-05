using GigAuth.Domain.Entities;

namespace GigAuth.Domain.Repositories.Permissions;

public interface IPermissionWriteOnlyRepository
{
    Task Add(Permission permission);
    Task<Permission?> GetById(Guid id);
    void Delete(Permission permission);
}