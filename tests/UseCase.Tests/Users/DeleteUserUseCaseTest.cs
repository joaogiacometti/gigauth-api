using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Users;
using GigAuth.Application.UseCases.Users.Delete;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Users;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(user.Id));

        Assert.Null(exception);
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

    private static DeleteUserUseCase CreateUseCase(User? userToDelete = null)
    {
        var writeRepository = new UserWriteOnlyRepositoryBuilder().GetById(userToDelete).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new DeleteUserUseCase(writeRepository, unitOfWork);
    }
}