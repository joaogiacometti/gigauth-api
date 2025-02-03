using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Permissions;
using Moq;

namespace CommonTestsUtilities.Repositories.Permissions;

public class PermissionReadOnlyRepositoryBuilder
{
    private readonly Mock<IPermissionReadOnlyRepository> _repository = new();

    public PermissionReadOnlyRepositoryBuilder GetFiltered(List<Permission>? permissions = null)
    {
        if (permissions is not null)
            _repository.Setup(r => r.GetFiltered(It.IsAny<RequestPermissionFilter>())).ReturnsAsync(permissions);
        else
            _repository.Setup(r => r.GetFiltered(It.IsAny<RequestPermissionFilter>())).ReturnsAsync([]);

        return this;
    }

    public PermissionReadOnlyRepositoryBuilder GetById(Permission? permission = null)
    {
        if (permission is not null)
            _repository.Setup(r => r.GetById(permission.Id)).ReturnsAsync(permission);

        return this;
    }

    public PermissionReadOnlyRepositoryBuilder GetByName(Permission? permission = null)
    {
        if (permission is not null)
            _repository.Setup(r => r.GetByName(permission.Name)).ReturnsAsync(permission);

        return this;
    }

    public IPermissionReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}