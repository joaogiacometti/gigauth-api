using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Users;
using GigAuth.Application.UseCases.Users.Get;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Users;

public class GetUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(user.Id);

        Assert.NotNull(result);
        Assert.Equivalent(result.Id, user.Id);
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var id = Guid.NewGuid();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ResourceErrorMessages.USER_NOT_FOUND, exception.Message);
    }

    private static GetUserUseCase CreateUseCase(User? userToGet = null)
    {
        var readRepository = new UserReadOnlyRepositoryBuilder()
            .GetById(userToGet).Build();

        return new GetUserUseCase(readRepository);
    }
}