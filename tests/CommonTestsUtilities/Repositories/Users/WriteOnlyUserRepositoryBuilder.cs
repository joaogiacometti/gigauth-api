using GigAuth.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories.Users;

public class WriteOnlyUserRepositoryBuilder
{
    private readonly Mock<IWriteOnlyUserRepository> _repository = new ();
    
    public IWriteOnlyUserRepository Build() => _repository.Object;
}