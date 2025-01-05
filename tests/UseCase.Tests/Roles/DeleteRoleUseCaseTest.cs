using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Roles;
using FluentAssertions;
using GigAuth.Application.UseCases.Roles.Delete;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Roles;

public class DeleteRoleUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var role = RoleBuilder.Build();

        var useCase = CreateUseCase(roleToDelete: role);
        
        var act = async () => await useCase.Execute(role.Id);

        await act.Should().NotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Role_Not_Found()
    {
        var id = Guid.NewGuid();
        
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.ROLE_NOT_FOUND));
    }
    
    private static DeleteRoleUseCase CreateUseCase(Role? roleToDelete = null)
    {
        var writeRepository = new RoleWriteOnlyRepositoryBuilder().GetById(roleToDelete).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new DeleteRoleUseCase(writeRepository, unitOfWork);
    }
}