using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Permissions;
using FluentAssertions;
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

        var useCase = CreateUseCase(permissionToDelete: permission);
        
        var act = async () => await useCase.Execute(permission.Id);

        await act.Should().NotThrowAsync();
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
    
    private static DeletePermissionUseCase CreateUseCase(Permission? permissionToDelete = null)
    {
        var writeRepository = new PermissionWriteOnlyRepositoryBuilder().GetById(permissionToDelete).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new DeletePermissionUseCase(writeRepository, unitOfWork);
    }
}