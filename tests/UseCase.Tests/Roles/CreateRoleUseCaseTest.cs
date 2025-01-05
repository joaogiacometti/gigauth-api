using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Repositories.Roles;
using CommonTestsUtilities.Requests.Roles;
using FluentAssertions;
using GigAuth.Application.UseCases.Roles.Create;
using GigAuth.Domain.Entities;
using GigAuth.Exception.ExceptionBase;
using GigAuth.Exception.Resources;

namespace UseCase.Tests.Roles;

public class CreateRoleUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestCreateRoleBuilder.Build();
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Name_Already_Used()
    {
        var role = RoleBuilder.Build();

        var request = RequestCreateRoleBuilder.Build();
        request.Name = role.Name;

        var useCase = CreateUseCase(roleWithName: role);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_ALREADY_USED));
    }

    [Fact]
    public async Task Error_Validation()
    {
        var request = RequestCreateRoleBuilder.Build();
        request.Name = "a";

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex =>
            ex.GetErrorList().Count == 1 && ex.GetErrorList().Contains(ResourceErrorMessages.NAME_TOO_SHORT));
    }

    private static CreateRoleUseCase CreateUseCase(Role? roleWithName = null)
    {
        var readRepository = new RoleReadOnlyRepositoryBuilder()
            .GetByName(roleWithName)
            .Build();
        var writeRepository = new RoleWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new CreateRoleUseCase(readRepository, writeRepository, unitOfWork);
    }
}