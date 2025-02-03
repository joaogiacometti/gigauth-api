using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Permissions;
using CommonTestsUtilities.Requests.Permissions;
using GigAuth.Application.UseCases.Permissions.Update;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Permissions;

public class UpdatePermissionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var permission = PermissionBuilder.Build();
        var request = RequestPermissionBuilder.Build();
        var useCase = CreateUseCase(permission);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(permission.Id, request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Permission_Not_Found()
    {
        var request = RequestPermissionBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.PERMISSION_NOT_FOUND);
    }

    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var permissionToUpdate = PermissionBuilder.Build();
        var permissionWithName = PermissionBuilder.Build();

        var request = RequestPermissionBuilder.Build();
        request.Name = permissionWithName.Name;

        var useCase = CreateUseCase(permissionToUpdate, permissionWithName);

        var act = async () => await useCase.Execute(permissionToUpdate.Id, request);

        var exception = await Assert.ThrowsAsync<AlreadyUsedException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_ALREADY_USED);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestPermissionBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_TOO_SHORT);
    }

    private static UpdatePermissionUseCase CreateUseCase(Permission? permissionToUpdate = null,
        Permission? permissionWithName = null)
    {
        var readRepository = new PermissionReadOnlyRepositoryBuilder()
            .GetByName(permissionWithName)
            .Build();
        var writeRepository = new PermissionWriteOnlyRepositoryBuilder()
            .GetById(permissionToUpdate)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new UpdatePermissionUseCase(writeRepository, readRepository, unitOfWork);
    }
}