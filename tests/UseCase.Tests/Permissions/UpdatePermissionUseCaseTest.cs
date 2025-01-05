using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Permissions;
using CommonTestsUtilities.Requests.Permissions;
using FluentAssertions;
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

        var act = async () => await useCase.Execute(permission.Id, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Permission_Not_Found()
    {
        var request = RequestPermissionBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.PERMISSION_NOT_FOUND));
    }
    
    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var permissionToUpdate = PermissionBuilder.Build();
        var permissionWithName = PermissionBuilder.Build();

        var request = RequestPermissionBuilder.Build();
        request.Name = permissionWithName.Name;

        var useCase = CreateUseCase(permissionToUpdate: permissionToUpdate, permissionWithName: permissionWithName);

        var act = async () => await useCase.Execute(permissionToUpdate.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_ALREADY_USED));
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestPermissionBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_TOO_SHORT));
    }

    private static UpdatePermissionUseCase CreateUseCase(Permission? permissionToUpdate = null, Permission? permissionWithName = null)
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