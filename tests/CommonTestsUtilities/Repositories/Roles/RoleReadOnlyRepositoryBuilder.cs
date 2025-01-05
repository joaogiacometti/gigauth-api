using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Roles;
using Moq;

namespace CommonTestsUtilities.Repositories.Roles;

public class RoleReadOnlyRepositoryBuilder
{
    private readonly Mock<IRoleReadOnlyRepository> _repository = new();

    public RoleReadOnlyRepositoryBuilder GetFiltered(List<Role>? roles = null)
    {
        if (roles is not null)
            _repository.Setup(r => r.GetFiltered(It.IsAny<RequestRoleFilter>())).ReturnsAsync(roles);
        else
            _repository.Setup(r => r.GetFiltered(It.IsAny<RequestRoleFilter>())).ReturnsAsync([]);

        return this;
    }
    
    public IRoleReadOnlyRepository Build() => _repository.Object;
}