using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.Roles;
using Moq;

namespace CommonTestsUtilities.Repositories.Roles;

public class RoleWriteOnlyRepositoryBuilder
{
    private readonly Mock<IRoleWriteOnlyRepository> _repository = new();

    public RoleWriteOnlyRepositoryBuilder GetById(Role? role = null)
    {
        if (role is not null)
            _repository.Setup(r => r.GetById(role.Id)).ReturnsAsync(role);

        return this;
    }

    public IRoleWriteOnlyRepository Build()
    {
        return _repository.Object;
    }
}