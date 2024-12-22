using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Users;
using FluentAssertions;
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

        var useCase = CreateUseCase(userToDelete: user);
        
        var act = async () => await useCase.Execute(user.Id);

        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Error_User_Not_Found()
    {
        var id = Guid.NewGuid();
        
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.USER_NOT_FOUND));
    }
    
    private static DeleteUserUseCase CreateUseCase(User? userToDelete = null)
    {
        var writeRepository = new UserWriteOnlyRepositoryBuilder().GetById(userToDelete).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new DeleteUserUseCase(writeRepository, unitOfWork);
    }
}