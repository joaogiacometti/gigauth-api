using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Permissions;
using CommonTestsUtilities.Requests.Permissions;
using GigAuth.Application.UseCases.Permissions.Create;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Permissions;

public class CreatePermissionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestPermissionBuilder.Build();
        var useCase = CreateUseCase();

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var permission = PermissionBuilder.Build();

        var request = RequestPermissionBuilder.Build();
        request.Name = permission.Name;

        var useCase = CreateUseCase(permission);

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<AlreadyUsedException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_ALREADY_USED);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestPermissionBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_TOO_SHORT);
    }

    private static CreatePermissionUseCase CreateUseCase(Permission? permissionWithName = null)
    {
        var readRepository = new PermissionReadOnlyRepositoryBuilder()
            .GetByName(permissionWithName)
            .Build();
        var writeRepository = new PermissionWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new CreatePermissionUseCase(readRepository, writeRepository, unitOfWork);
    }
}