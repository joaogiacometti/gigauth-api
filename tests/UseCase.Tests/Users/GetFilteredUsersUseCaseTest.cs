using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Users;
using FluentAssertions;
using GigAuth.Application.UseCases.Users.GetFiltered;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Users;

public class GetFilteredUsersTest
{
    [Fact]
    public async Task Success()
    {
        var usersToGet = UserBuilder.BuildList();
        var request = RequestUserFilterBuilder.Build();

        var useCase = CreateUseCase(usersToGet: usersToGet);

        var result = await useCase.Execute(request);

        result.Should().NotBeNullOrEmpty();
        result!.Count.Should().Be(usersToGet.Count);
    }
    
    [Fact]
    public async Task Success_No_Content()
    {
        var request = RequestUserFilterBuilder.Build();
        request.UserName = "toospecificusernamenotfound";

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result!.Count.Should().Be(0);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestUserFilterBuilder.Build();
        request.UserName = "";

        var useCase = CreateUseCase();

        var act = async() => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        
        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.USER_NAME_EMPTY));
    }
    
    private static GetFilteredUsersUseCase CreateUseCase(List<User>? usersToGet = null)
    {
        var readRepository = new UserReadOnlyRepositoryBuilder()
            .GetFiltered(usersToGet).Build();

        return new GetFilteredUsersUseCase(readRepository);
    }
}