using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Roles;
using CommonTestsUtilities.Requests.Roles;
using GigAuth.Application.UseCases.Roles.Create;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Roles;

public class CreateRoleUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRoleBuilder.Build();
        var useCase = CreateUseCase();

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var role = RoleBuilder.Build();

        var request = RequestRoleBuilder.Build();
        request.Name = role.Name;

        var useCase = CreateUseCase(role);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<AlreadyUsedException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_ALREADY_USED);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestRoleBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_TOO_SHORT);
    }

    private static CreateRoleUseCase CreateUseCase(Role? roleWithName = null)
    {
        var readRepository = new RoleReadOnlyRepositoryBuilder()
            .GetByName(roleWithName)
            .Build();
        var writeRepository = new RoleWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new CreateRoleUseCase(readRepository, writeRepository, unitOfWork);
    }
}