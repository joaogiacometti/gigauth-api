using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.Permissions;
using Moq;

namespace CommonTestsUtilities.Repositories.Permissions;

public class PermissionWriteOnlyRepositoryBuilder
{
    private readonly Mock<IPermissionWriteOnlyRepository> _repository = new();

    public PermissionWriteOnlyRepositoryBuilder GetById(Permission? permission = null)
    {
        if (permission is not null)
            _repository.Setup(r => r.GetById(permission.Id)).ReturnsAsync(permission);

        return this;
    }

    public IPermissionWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}