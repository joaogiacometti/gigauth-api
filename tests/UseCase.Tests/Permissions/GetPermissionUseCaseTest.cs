using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Permissions;
using GigAuth.Application.UseCases.Permissions.Get;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Permissions;

public class GetPermissionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var permission = PermissionBuilder.Build();

        var useCase = CreateUseCase(permission);

        var result = await useCase.Execute(permission.Id);

        Assert.NotNull(result);
        Assert.Equal(result.Id, permission.Id);
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

    private static GetPermissionUseCase CreateUseCase(Permission? permissionToGet = null)
    {
        var readRepository = new PermissionReadOnlyRepositoryBuilder()
            .GetById(permissionToGet).Build();

        return new GetPermissionUseCase(readRepository);
    }
}