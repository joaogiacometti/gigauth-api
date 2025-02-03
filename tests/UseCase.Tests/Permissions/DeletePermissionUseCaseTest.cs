using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Permissions;
using GigAuth.Application.UseCases.Permissions.Delete;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Permissions;

public class DeletePermissionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var permission = PermissionBuilder.Build();

        var useCase = CreateUseCase(permission);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(permission.Id));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Permission_Not_Found()
    {
        var id = Guid.NewGuid();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ResourceErrorMessages.PERMISSION_NOT_FOUND, exception.Message);
    }

    private static DeletePermissionUseCase CreateUseCase(Permission? permissionToDelete = null)
    {
        var writeRepository = new PermissionWriteOnlyRepositoryBuilder().GetById(permissionToDelete).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new DeletePermissionUseCase(writeRepository, unitOfWork);
    }
}