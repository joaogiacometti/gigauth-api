using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Users;
using CommonTestsUtilities.Requests.Filters;
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

        var useCase = CreateUseCase(usersToGet);

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equivalent(result.Count, usersToGet.Count);
    }

    [Fact]
    public async Task Success_No_Content()
    {
        var request = RequestUserFilterBuilder.Build();
        request.UserName = "toospecificusernamenotfound";

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equivalent(result.Count, 0);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestUserFilterBuilder.Build();
        request.UserName = "";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.USER_NAME_EMPTY);
    }

    private static GetFilteredUsersUseCase CreateUseCase(List<User>? usersToGet = null)
    {
        var readRepository = new UserReadOnlyRepositoryBuilder()
            .GetFiltered(usersToGet).Build();

        return new GetFilteredUsersUseCase(readRepository);
    }
}