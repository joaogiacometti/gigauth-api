using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories.Permissions;
using FluentAssertions;
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

        var useCase = CreateUseCase(permissionToGet: permission);

        var result = await useCase.Execute(permission.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(permission.Id);
    }
    
    [Fact]
    public async Task Error_Permission_Not_Found()
    {
        var id = Guid.NewGuid();
        
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.PERMISSION_NOT_FOUND));
    }
    
    private static GetPermissionUseCase CreateUseCase(Permission? permissionToGet = null)
    {
        var readRepository = new PermissionReadOnlyRepositoryBuilder()
            .GetById(permissionToGet).Build();

        return new GetPermissionUseCase(readRepository);
    }
}