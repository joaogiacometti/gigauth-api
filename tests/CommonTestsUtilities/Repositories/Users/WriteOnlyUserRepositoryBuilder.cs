using GigAuth.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories.Users;

public class WriteOnlyUserRepositoryBuilder
{
    private readonly Mock<IUserWriteOnlyRepository> _repository = new ();
    
    public IUserWriteOnlyRepository Build() => _repository.Object;
}