using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories.Users;

public class WriteOnlyUserRepositoryBuilder
{
    private readonly Mock<IUserWriteOnlyRepository> _repository = new ();
    
    public WriteOnlyUserRepositoryBuilder GetById(User? user)
    {
        if (user is not null)
            _repository.Setup(r => r.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }
    
    public IUserWriteOnlyRepository Build() => _repository.Object;
}