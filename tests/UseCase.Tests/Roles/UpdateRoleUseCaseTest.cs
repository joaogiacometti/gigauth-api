using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Roles;
using CommonTestsUtilities.Requests.Roles;
using FluentAssertions;
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

        var act = async () => await useCase.Execute(role.Id, request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Role_Not_Found()
    {
        var request = RequestRoleBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.ROLE_NOT_FOUND));
    }
    
    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var roleToUpdate = RoleBuilder.Build();
        var roleWithName = RoleBuilder.Build();

        var request = RequestRoleBuilder.Build();
        request.Name = roleWithName.Name;

        var useCase = CreateUseCase(roleToUpdate: roleToUpdate, roleWithName: roleWithName);

        var act = async () => await useCase.Execute(roleToUpdate.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_ALREADY_USED));
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestRoleBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(Guid.NewGuid(), request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_TOO_SHORT));
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