using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Roles;
using CommonTestsUtilities.Requests.Filters;
using GigAuth.Application.UseCases.Roles.GetFiltered;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Roles;

public class GetFilteredRolesTest
{
    [Fact]
    public async Task Success()
    {
        var rolesToGet = RoleBuilder.BuildList();
        var request = RequestRoleFilterBuilder.Build();

        var useCase = CreateUseCase(rolesToGet);

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equivalent(result.Count, rolesToGet.Count);
    }

    [Fact]
    public async Task Success_No_Content()
    {
        var request = RequestRoleFilterBuilder.Build();
        request.Name = "toospecificnamenotfound";

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equivalent(result.Count, 0);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestRoleFilterBuilder.Build();
        request.Name = "";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_EMPTY);
    }

    private static GetFilteredRolesUseCase CreateUseCase(List<Role>? rolesToGet = null)
    {
        var readRepository = new RoleReadOnlyRepositoryBuilder()
            .GetFiltered(rolesToGet).Build();

        return new GetFilteredRolesUseCase(readRepository);
    }
}