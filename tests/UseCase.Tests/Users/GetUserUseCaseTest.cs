using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Users;
using FluentAssertions;
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

        var useCase = CreateUseCase(userToGet: user);

        var result = await useCase.Execute(user.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
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
    
    private static GetUserUseCase CreateUseCase(User? userToGet = null)
    {
        var readRepository = new UserReadOnlyRepositoryBuilder()
            .GetById(userToGet).Build();

        return new GetUserUseCase(readRepository);
    }
}