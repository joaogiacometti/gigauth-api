using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Roles;
using CommonTestsUtilities.Requests.Roles;
using GigAuth.Application.UseCases.Roles.Update;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Roles;

public class UpdateRoleUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var role = RoleBuilder.Build();
        var request = RequestRoleBuilder.Build();
        var useCase = CreateUseCase(role);

        var exception = await Record.ExceptionAsync(async () => await useCase.Execute(role.Id, request));

        Assert.Null(exception);
    }

    [Fact]
    public async Task Error_Role_Not_Found()
    {
        var request = RequestRoleBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ResourceErrorMessages.ROLE_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var roleToUpdate = RoleBuilder.Build();
        var roleWithName = RoleBuilder.Build();

        var request = RequestRoleBuilder.Build();
        request.Name = roleWithName.Name;

        var useCase = CreateUseCase(roleToUpdate, roleWithName);

        var act = async () => await useCase.Execute(roleToUpdate.Id, request);

        var exception = await Assert.ThrowsAsync<AlreadyUsedException>(act);
        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_ALREADY_USED);
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestRoleBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);

        Assert.Contains(exception.GetErrorList(), ex => ex == ResourceErrorMessages.NAME_TOO_SHORT);
    }

    private static UpdateRoleUseCase CreateUseCase(Role? roleToUpdate = null, Role? roleWithName = null)
    {
        var readRepository = new RoleReadOnlyRepositoryBuilder()
            .GetByName(roleWithName)
            .Build();
        var writeRepository = new RoleWriteOnlyRepositoryBuilder()
            .GetById(roleToUpdate)
            .Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new UpdateRoleUseCase(writeRepository, readRepository, unitOfWork);
    }
}