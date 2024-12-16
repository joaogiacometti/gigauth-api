using GigAuth.Domain.Entities;
using GigAuth.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories.Users;

public class ReadOnlyUserRepositoryBuilder
{
    private readonly Mock<IReadOnlyUserRepository> _repository = new();

    public ReadOnlyUserRepositoryBuilder GetByUserName(User? user)
    {
        if (user is not null)
            _repository.Setup(r => r.GetByUserName(user.UserName)).ReturnsAsync(user);

        return this;
    }
    
    public ReadOnlyUserRepositoryBuilder GetByEmail(User? user)
    {
        if (user is not null)
            _repository.Setup(r => r.GetByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }
    
    public IReadOnlyUserRepository Build() => _repository.Object;
}