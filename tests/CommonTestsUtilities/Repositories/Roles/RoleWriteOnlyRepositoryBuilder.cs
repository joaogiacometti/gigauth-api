using GigAuth.Domain.Repositories.Roles;
using Moq;

namespace CommonTestsUtilities.Repositories.Roles;

public class RoleWriteOnlyRepositoryBuilder
{
    private readonly Mock<IRoleWriteOnlyRepository> _repository = new ();
    
    public IRoleWriteOnlyRepository Build() => _repository.Object;
}