using GigAuth.Domain.Entities;
using GigAuth.Domain.Filters;
using GigAuth.Domain.Repositories.Users;
using Moq;

namespace CommonTestsUtilities.Repositories.Users;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository = new();

    public UserReadOnlyRepositoryBuilder GetFiltered(List<User>? users = null)
    {
        if (users is not null)
            _repository.Setup(r => r.GetFiltered(It.IsAny<RequestUserFilter>())).ReturnsAsync(users);
        else
            _repository.Setup(r => r.GetFiltered(It.IsAny<RequestUserFilter>())).ReturnsAsync([]);

        return this;
    }
    
    public UserReadOnlyRepositoryBuilder GetById(User? user = null)
    {
        if (user is not null)
            _repository.Setup(r => r.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }
    
    public UserReadOnlyRepositoryBuilder GetByUserName(User? user = null)
    {
        if (user is not null)
            _repository.Setup(r => r.GetByUserName(user.UserName)).ReturnsAsync(user);

        return this;
    }
    
    public UserReadOnlyRepositoryBuilder GetByEmail(User? user = null)
    {
        if (user is not null)
            _repository.Setup(r => r.GetByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }
    
    public IUserReadOnlyRepository Build() => _repository.Object;
}